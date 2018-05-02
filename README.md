# Litium Smart Image Add-on - Google Cloud Vision
An Add-on for Litium platform, which analyzes and extracts rich information from images, powered by Google Cloud Vision, to categorize images in a better way.

## Install
To install this Add-on to your site, you need a site running equivalent version of Litium. This Add-on's version is inlined with Litium's version. If its version is 6.1.0, that means it is built against Litium 6.1.0. 

Install the NuGet package to your project by executing the following command in Package Manager Console:
`Install-Package Litium.AddOns.SmartImage.GoogleCloudVision`

## How to use
First, we need to set up Google authentication. Please follow this link for more detail of how to Create service account and how to set the environment variable to point to the path of the JSON file: https://cloud.google.com/vision/docs/libraries
A field named 'Smart Image tags' is added to the system by default. We need to add this field to the Image template. So extracted information from images will be stored in this field. To add it to Image template, navigate to the Template setting and choose the field in the field select box.
When an image is uploaded, after a few seconds, the data will be extracted and added to the appropriate field of the file.

## Build from source
Even though the AddOn is ready to use by just installing the package, you can still build it from source.
### Requirement
1. Make sure you have set up Litium NuGet feed https://docs.litium.com/download/litium-nuget-feed
2. You need a site running equivalent version of Litium. This Add-on's version is inlined with Litium's version. If its version is 6.1.0, that means it is built against Litium 6.1.0.
3. You need to have NPM installed. [How to install NPM](https://www.npmjs.com/get-npm)

### Build
1. Open the solution file Litium.AddOns.SmartImage.GoogleCloudVision.sln
2. Execute the following command in Package Manager Console: `Update-Package -ReInstall`
4. Build the solution

The assembly file named Litium.AddOns.SmartImage.GoogleCloudVision.dll should be built.