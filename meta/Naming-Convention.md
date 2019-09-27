# Naming Convention

## 1. Classes, Enums, Structures, Namespaces, Delegates

- Upper camel case, public or internal, nested or not. E.g. `ClassExample`.

## 2. Interfaces

- Upper camel case, with prefix `I`. E.g. `IInterfaceSomething`.

## 3. Class members

### 3.1 Static members and constants

- Upper camel case.

### 3.2 Instance members

#### 3.2.1 Public and protected instance members

- Upper camel case

#### 3.2.2 Private instance members

##### 3.2.2.1 Private instance fields

- Lower camel case, with prefix `_`. E.g. `_fieldAlpha`.

##### 3.2.2.2 Private instance properties

- Upper camel case.

##### 3.2.2.3 Private instance methods

- Lower camel case. E.g. `calculateSomething`.

##### 3.2.2.3 Private instance events

- Upper camel case, though we shall never use private instance events.

## 4. Local variables

- Lower camel case.

## 5. Parameters

- Lower camel case.

## 6. Type parameters

- Upper camel case, with prefix `T`. E.g. `TSource`.

## 7. Enum members

- Upper camel case.

## A1. Other conventions

- Asynchronous methods should be named with suffix `Async`, unless it's an entry point.
- Members of anonymous classes can be named using the Lower camel case.