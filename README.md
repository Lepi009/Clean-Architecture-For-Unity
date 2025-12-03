# A Unity Template for Clean Architecture
This is the repo to my master's thesis "Modular Game Architecture in Unity: A Case Study of Clean Architecture and Platform Optimization in Unbreachable".



## Abstract of my thesis
Publishing games across multiple platforms is becoming more attractive. However, it introduces significant architectural complexity due to requirements and different APIs. This thesis addresses the challenge of designing a modular, maintainable software architecture for multi-platform Unity games. Using the Design Science Research methodology, an architectural artifact is designed, implemented, and evaluated using a real-world use case, Unbreachable, an ongoing game project by Buckfish.

The designed architecture uses established design patterns combined with principles of Clean Architecture. This includes event-driven communication, dependency injection, and the Model-View-Presenter pattern. The use case was evaluated using code metrics, Windows and Steam Deck runtime benchmarks, and tests in the Unity Editor.

The results indicate that the software architecture improves maintainability and scalability, while still providing a desired performance. These findings demonstrate that architectural principles can be effectively applied to game development in Unity projects. The research contributes a validated software architecture, a Unity template, and empirical insights in developing scalable and maintainable games.


# Layers
Layering code into clear concerns helps improve code quality and maintainability. This separation reduces coupling and ensures that gameplay rules are independent from Unity-specific implementations. Unit testing can be done effectively outside of the Unity runtime. The layered architecture creates a more modular, flexible, and long-term sustainable codebase.

## Domain Layer
The domain layer is equal to the entity layer in the clean architecture (see  \autoref{sec:cleanArchitecture}). It includes the most robust rules, objects, and behavior. This layer is entirely self-contained and has no dependencies on any layers above it. A change in this layer often requires a complete recompilation of all other scripts (see \autoref{designAssemblies}).

This layer has no references to other assemblies, especially no UnityEngine classes or functions are available. Thus, commonly used structs and classes such as rects, vectors, and matrices are implemented here, since the Unity ones are not available. Further, there are the [ServiceLocator](Assets/Scripts/01-Domain/Basics/ServiceLocator.cs) and the [event bus](Assets/Scripts/01-Domain/Event%20System).

## Application Layer
The application layer is the second layer and is equally responsible to the second layer in clean architecture. This layer only depends on the domain layer. It contains all use cases, executes the game logic, and implements abstractions.

Since the use cases are game-dependent, there are no clear classes that are always contained in the application layer. Games with an in-game shop could implement a shop service to handle item purchases. This shop service is implemented in the second layer. Also, an InventoryService that adds or removes items could be implemented here. The inventory itself would be implemented in the domain layer. Any quest service could also be implemented here to update quest progress based on events.

One further purpose of this layer is to define the interfaces implemented by layers above it. Through dependency injection, the concrete implementation is injected at runtime. Those interfaces should often have asynchronous functions. Because the concrete implementation could be asynchronous, this must be considered in those interfaces and in how they are used in this layer. For example, reading a file can be handled synchronously; however, retrieving a file from the cloud must be done asynchronously and may fail. The service's interface must handle the most complex case. Otherwise, the concrete implementation influences a service's behaviour, which violates the Liskov Substitution Principle.

Additionally, the implemented services fire specific events that the layers above can subscribe to. A heavy-event-driven service could be the audio system, because it needs to adapt the music to the current state of the game and respond to many different events by triggering sound effects. For instance, a change in the player's money wallet could trigger a buying or selling sound.

## Infrastructure Layer
The infrastructure layer serves as the translation layer and is similar to the third layer of the clean architecture. This is the first layer that has access to the Unity engine. 

In the third layer, the application-layer interfaces are implemented. The concrete implementations may vary between platforms. Thus, often multiple implementations are found for a single interface.

One of the popular classes is data transfer objects (DTO). Those are used to get data and translate it into the corresponding domain entities. This translation keeps the domain independent of the providing services. For instance, changing a name in a JSON file only affects the DTO, not the domain entity.

Another major group of classes is the presenters. Those are part of the MVP pattern, which is the [UI system](https://github.com/Lepi009/Clean-Architecture-Unity-Template/edit/master/README.md#user-interface). These can also be seen as a translator between the core application and the concrete user interface.

## Presentation Layer
The outermost layer is the presentation layer. This layer has been adapted, so it is not as similar to the clean architecture's fourth layer as the other layers are to their restrictive layer.

This layer is completely Unity-dependent. All necessary game objects and MonoBehaviours are in this layer. This layer often changes due to new user interface adaptations, input improvements, and graphics updates. Those do not influence the layer beneath.

Mentionable classes implemented in this layer include the views of the MVP design (\autoref{Design/UI}) and a Unity-dependent async provider (\autoref{Design/Async}).

## Misc
There are additional classes, that are out of that layers. Those include the [bootstrapper](https://github.com/Lepi009/Clean-Architecture-Unity-Template/edit/master/README.md#bootstrapper), and the custom build script.

# Core Systems
## Event Bus
## Bootstrapper
## Service Locator
## Async Operations using Coroutines
## Platform adapters

# User Interface


# Input



# How to Build

