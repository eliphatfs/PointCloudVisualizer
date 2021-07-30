# Point Cloud Visualizer
## Requirements
Developed and tested on Unity 2019.4.18f1c1, but it should work with newer versions of unity.

## Example
Start unity and open `Main` scene. It contains an example setup and can be freely tweaked.

## Usage
### Rendering Point Clouds (Basic)
To render a point cloud, follow the following steps:
1. Generate a `.json` file from your source of point cloud data and place it somewhere below the `Assets` folder of the project (for NumPy arrays you may call the `tolist` method to obtain an object compatible with the python built-in json library). The `.json` file should contain an array of objects, each object describing a point cloud for example:
```json
[
    {
        "points": [[1, 2, 0], [-1, 2, 1], ...]
    },
    {
        "points": ...
    }
]
```
2. Start main scene and create an empty object under `World Root`. Attach a `Point Cloud Root` component to the object. Assign the `.json` file containing your point clouds to the `Data Source` property. Hit `Add Render Key` and enter the property key `"points"` (without quotes) in the `Key` field. Enter the index in the `.json` list of the object you want to render. Assign a point size according to your input data (Typically 0.01 to 0.04 if input data ranges from -1 to 1).
3. Hit the play button. Change the transform (position, rotation and scale) of the point cloud root object to comfort yourself. You may clone the root object and assign different object indices as well as transform values to render multiple point clouds in a row.
4. If you want to save the result to a `.png` file, you may assign a resolution in the game window, find `Screenshot Manager` and hit `Toggle Screenshot`. The `Scaling` field may be assigned in addition to the game window resolution for super-resolution rendering.

### Advanced Options
Currently advanced options mainly deal with coloring of point clouds which contribute a lot to visualizations.

The documentation is on its way. Please refer to the example at the moment.

### Lighting
All lighting is made out of built-in unity stuff and can be tweaked to your preference.

### Extension and Interaction
There is an option named `Enable Physics` per render key. If selected, the points generated in the render key will be equipped with colliders so that they may be easily accessed with the physics system, for example raycasting on clicking. An example that exchanges points between selections (the blue and the red one on extremes of the scene) is included.

Other functional extensions can also be implemented via the unity scripting system.

### Helper Scripts
A component named `Transform Snapshot` is included in the project. It is designed to load and store local transform values so that play mode changes of these parameters may be saved back in edit mode. Note that the component shares storage between instances and the data will be lost after a reload.

## Contributing
Start an issue if you want some feature that is currently missing. Submit a pull request if you would like to implement new stuff. Contributions are welcome.
