# Software Architecture Sketch

## Tree

src <br>
├── API <br>
│   ├── _TODO_ <br>
├── Common <br>
│   ├── Application <br>
│   ├── Architecture <br>
│   ├── Infrastructure <br>
│   ├── Domain <br>
│   └── Tests <br>
├── Modules <br>
│   ├── Administration <br>
│   │   ├── Application <br>
│   │   ├── Architecture <br>
│   │   ├── Infrastructure <br>
│   │   ├── Domain <br>
│   │   └── Tests <br>
│   └── ... 
└── ... 

## Patterns TODO

Simple types (constrained) with smart constructors 'fromX'
Compound types (constrained)
Private / internal / public types modules and functions
Value objects?
Entity
Aggregate root
Business rule validation = impossible state enforced by types + validation error mapping
Exception for low level errors and immediate failure
Domain event