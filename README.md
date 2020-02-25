# Monica OGCServiceCatalogue
<!-- Short description of the project. -->

OGCServiceCatralogue manages the OGC based Service Catalogue

<!-- A teaser figure may be added here. It is best to keep the figure small (<500KB) and in the same repo -->

## Getting Started
<!-- Instruction to make the project up and running. -->

The project documentation is available on the [Wiki](https://github.com/MONICA-Project/template/wiki).

## Deployment
<!-- Deployment/Installation instructions. If this is software library, change this section to "Usage" and give usage examples -->

### Docker
To run the latest version of foobar:
```bash
docker run -p 8090:80 -e GOSTServerAddress=http://127.0.0.1:8080/v1.0/ -e GOSTPrefix= -e MQTTServerAddress=127.0.0.1:1883 monicaproject/ogcservicecatalogue
```
### Environment Variables
These variables are **compulsory**, the service will fail to start if these are not defined.
* **GOSTServerAddress** the URL for accessing the GOST server. **NB!** it must include /v1.0/ 
* **GOSTPrefix** the prefix used for MQTT messages to be stored in GOST
* **MQTTServerAddress** The address of the MQTT broker
## Development
<!-- Developer instructions. -->

### Prerequisite
This projects depends on xyz. Installation instructions are available [here](https://xyz.com)

On Debian:
```bash
apt install xyz
```

### Test
Use tests.sh to run unit tests:
```bash
sh tests.sh
```

### Build

```bash
g++ -o app app.cpp
```

## Contributing
Contributions are welcome. 

Please fork, make your changes, and submit a pull request. For major changes, please open an issue first and discuss it with the other authors.

## Affiliation
![MONICA](https://github.com/MONICA-Project/template/raw/master/monica.png)  
This work is supported by the European Commission through the [MONICA H2020 PROJECT](https://www.monica-project.eu) under grant agreement No 732350.

> # Notes
>
> * The above templace is adapted from [[1](https://github.com/cpswarm/template), [2](https://www.makeareadme.com), [3](https://gist.github.com/PurpleBooth/109311bb0361f32d87a2), [4](https://github.com/dbader/readme-template)].
> * Versioning: Use [SemVer](http://semver.org/) and tag the repository with full version string. E.g. `v1.0.0`
> * License: Provide a LICENSE file at the top level of the source tree. You can use Github to [add a license](https://help.github.com/en/articles/adding-a-license-to-a-repository). This template repository has an [Apache 2.0](LICENSE) file.
>
> *Remove this section from the actual readme.*
