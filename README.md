# VulkanCpu

This a simple (ok, not so simple) software simulator for the Vulkan Graphics API written in C# .net4.5.

## What it can do?

The primary objective is to port to C# and run all the tutorials at https://vulkan-tutorial.com/Introduction

It can run the 9 tutorials present in the Example folder, including the tutorial #9 "Ex09_LoadingModels" that can load and display the huge "Chalet Hippolyte Chassande Baroz" model.

Missing tutorials:

- Generating MipMaps
- Multisampling

### Prerequisites

You can directly run tutorial/examples from Ex01 to Ex08.
Ex09 uses a mesh and texture file that are very large, so I decided to not commit them. Please download this requisites and update MODEL_PATH and TEXTURE_PATH variables according.

Model: https://vulkan-tutorial.com/resources/chalet.obj.zip (please point the path to the decompressed model file)
Texture: https://vulkan-tutorial.com/resources/chalet.jpg

### Nice features

- Texture perspective correction
- Texture interpolation
- Pixel shader and vertex shader
- Optimized memory operations using pinned pointers (and other techniques) for maximum performance
- Optimized pipeline execution using C# compiled expressions for maximum performance
- Optimized shader execution using C# compiled expressions for maximum performance
- Optimized Windows form drawing for maximum performance

## Contributing

Feel free to contribute and/or send pull requests. :-)

## Authors

Initial work for the Vulkan software simulator - Bazoocaze (me!)

For the other libraries in this repo, I only ported the code to C#, but the original authors detain the original license:
- Stblib
- tinyobjloaderLib
- GlfwLib

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details
