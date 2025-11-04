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
    [x] Server must accept connection
    [x] Send message from client to server
    [x] Send message from server to client
    [x] Manually serialize 'AddPlayerPacket'
    [x] Manually serialize 'RemovePlayerPacket'
    [x] Serialize ^ when sent from server to client.
    [x] Remove player when packet arrives
    [x] Add player when pakcet arrives
    [ ] Replicate player position to server, and to other players.
        [ ] Confirm position being sent to server
        [ ] Confirm player pos being replicated from server to client
        [ ] Confirm correct number of players being shown on server and client
        [ ] Render some debug details about num of clients, etc
        [ ] Figure out why there's a third player on one client

    [ ] Log in to Server
        [ ] Get snapshot of players
        [ ] Spawn player
        [ ] Tell everyone player was spawned

## GOAL. Get something deployable and playable as fast as possible.

    [ ] Keep track of PlayerEntities
    [ ] Send server state snapshot
    [ ] Send player join/leave updates
[ ] Create two clients, control each and replicate each to one another

