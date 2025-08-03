C:\Users\Jackson\AppData\Local\Programs\ldtk\extraFiles\samples

Install this for codegen from ltdK:
dotnet dotnet tool install --global LDtkMonogame.Codegen --version 1.8.0


## PROGRESS LAST TIME (26/07/2025)
[x] Load a .ldtk file
[x] Load and control the player
[x] Fix animations
[x] Add collision to the level
[ ] Create a server state
    [x] Create Token API, generate token and get it on the client and send to server
    [ ] Server must accept connection
    [ ] Send message from client to server
    [ ] Send message from server to client
    [ ] Keep track of PlayerEntities
    [ ] Send server state snapshot
    [ ] Send player join/leave updates
[ ] Create two clients, control each and replicate each to one another