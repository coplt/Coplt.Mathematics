# Tests.Arm

The runner of the checks of the arm paths of the simd library on a machine that is not an arm one.

The checks themselves live in [`../Tests/Arith/MatrixTransposeArm.cs`](../Tests/Arith/MatrixTransposeArm.cs)
and this project compiles that file by a link rather than holding a copy of it, so the checks of a machine that is
an arm one are the checks of this runner beside the ones `dotnet test` reaches.

## Why another test project

The checks of the arm paths of the simd library reach the members of the library that the member of the value of
a matrix never reaches on a machine that is not an arm one, so they mirror those members and hold them against the
member of the value, and they are skipped (`the machine has no arm register`) everywhere else.

Running them on a real arm one is what the checks are for, and the machine at hand is an x86 one, so they are run
inside an emulated arm64 container:

- the `Tests` project is driven by the test sdk (`Microsoft.NET.Test.Sdk`), and the output of such a project does
  not run its checks where it is started, because the test host of the sdk drives them instead;
- this project holds the same checks beside a plain `Main` that the runner of NUnitLite reaches, so its output is
  an ordinary executable: it runs the checks wherever it is started, which makes it the one that an emulator can
  start.

## Running the checks of an arm machine

The commands below were the ones the checks were first run with, on a machine whose arm64 container is emulated by
`qemu-user-static` of the podman machine and whose image is `mcr.microsoft.com/dotnet/runtime-deps:10.0-alpine`
(an Alpine image, which is what the `linux-musl-arm64` target of the publish matches).

### One time for every start of the podman machine

The machine registers the emulator of an arm machine through `binfmt_misc`, which is a part of the kernel of the
machine and is therefore lost where the machine stops:

```pwsh
podman machine start
podman machine ssh "sudo mkdir -p /proc/sys/fs/binfmt_misc && sudo mount -t binfmt_misc binfmt_misc /proc/sys/fs/binfmt_misc && sudo sh -c 'cat /usr/lib/binfmt.d/qemu-aarch64-static.conf > /proc/sys/fs/binfmt_misc/register'"
```

The command needs `qemu-user-static` inside the machine, which the machine of Podman Desktop has by itself: check it
with `podman machine ssh "ls /usr/lib/binfmt.d/qemu-aarch64-static.conf"`, and install it where it is missing
(`podman machine ssh "sudo dnf install -y qemu-user-static"`).

The registration is the one that matters: without it the container of an arm one fails with
`exec container process '/bin/uname': Exec format error`, and with it the emulator of the machine answers
`podman run --rm --platform linux/arm64 alpine:latest uname -m` with `aarch64`.

### Every run

```pwsh
# the output of the runner, published for the kind of a machine that the image is, which is an Alpine arm64 one
dotnet publish Tests.Arm/Tests.Arm.csproj -c Release -r linux-musl-arm64 --self-contained true -o $env:TEMP\Tests.Arm

# the checks, run inside the emulated arm64 container: the published folder is mounted as /app
podman run --rm --platform linux/arm64 `
  -v $env:TEMP\Tests.Arm:/app -w /app `
  mcr.microsoft.com/dotnet/runtime-deps:10.0-alpine `
  ./Tests.Arm --noresult --labels=After
```

The mount takes the path of the published folder, which has to be an absolute one, and the arguments are the ones
of the runner of NUnitLite (`--noresult` keeps it from writing a result file, `--labels=After` prints the name of
every check).

The checks passing on the emulated arm machine read

```
Runtime Environment
   OS Version: Alpine Linux v3.24
  CLR Version: 10.0.12
...
Passed => Tests.Arith.TestMatrixTransposeArm.OfSquare
Passed => Tests.Arith.TestMatrixTransposeArm.OfTwoByTwo
Passed => Tests.Arith.TestMatrixTransposeArm.OfWideAndTall

Test Run Summary
  Overall result: Passed
  Test Count: 3, Passed: 3, Failed: 0, Warnings: 0, Inconclusive: 0, Skipped: 0
```

## Notes

- The runner of this project is not a test project of `dotnet test`: it does not reference the test sdk and it does
  not name itself a test project, so `dotnet test` of the solution leaves it alone, while the solution builds it
  beside everything else, which keeps the checks of the file it links compiled.
- On a machine that is an x86 one the checks of this runner are all skipped, so running it where it is built only
  answers that it is wired up; the emulated run above is the one that reads them.
- The checks mirror the members of the simd library over the registers of the columns of the value, and the mirror
  of a kind of four bytes reads the registers as floats (`AsSingle`) so that a single mirror serves every kind: a
  check of such a kind has to read the lanes back through the same kind (`AsInt32`, `AsUInt32`) rather than through
  the floats of the mirror, which are the bit patterns of the kind and not its values.
