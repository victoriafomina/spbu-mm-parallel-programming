## What is the app for?

The application implements Floyd-Warshall algorithm providing the possibility to run it both in a single thread and in parallel. Parallel implementation is based on Message Passing Interface (MPI).

## How to launch the app

### Environment
- .NET 7
- Microsoft MPI

### Launch
In order to launch the app, run

`<path-to-mpiexec.exe> -n <run-this-number-of-program-copies-on-given-nodes> <path-to-FloydWarshallAlgorithm.exe> <path-to-input-graph> <path-to-output-adjacency-matrix>`

On Windows the path to mpiexec.exe is usually `C:\Program Files\Microsoft MPI\Bin\mpiexec.exe`
