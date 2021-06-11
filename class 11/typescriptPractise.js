// Class
/* class Greeter {
    greeting: string;
    constructor(message: string) {
      this.greeting = message;
    }
    greet() {
      return "Hello, " + this.greeting;
    }
  }
  
  let greeter = new Greeter("world");

  console.log(greeter.greet()); */
// Interface Start
/* function printLabel(labeledObj: { label: string }) {
    console.log(labeledObj.label);
  }
  
  let myObj = { size: 10, label: "Size 10 Object" };
  printLabel(myObj); */
/* interface LabeledValue {
    label: string;
  }
  
  function printLabel(labeledObj: LabeledValue) {
    console.log(labeledObj.label);
  }
  
  let myObj = { size: 8, label: "Size 8 Object" };
printLabel(myObj); */
/* interface IPerson
{
    Name : string;
    Age: Number;
}

interface IEmployee extends IPerson
{
    EmpId : Number;
}

let obj : IEmployee =
{
    Name : "Asif",
    Age : 25,
    EmpId : 2
}

console.log(obj.EmpId); */
/* interface IEmployee
{
    name : string,
    empcode : number,
    getSalary : (number) => number;
}

class Employee implements IEmployee
{
    name: string;
    empcode: number;
    
    constructor(code : number, name : string)
    {
        this.name = name,
        this.empcode = code
    }

    getSalary(num : number) : number
    {
        return 2000;
    }
    
}

let obj = new Employee(1, "Asif");

console.log(obj.getSalary(1)); */
// Interface Finished
// Inheritance Started
/* class Animal
{
    move(distanceInMeter : number = 0)
    {
       console.log(`Animal moved ${distanceInMeter}m.`)
    }
}

class Dog extends Animal
{
  bark()
  {
    console.log("woff! woff!");
  }
}

const obj = new Dog();
obj.bark();
obj.move(10);
obj.bark();

class Animal
{
  name : string

  constructor(name : string)
  {
    this.name = name
  }

  move(distanceInMeters : number = 0)
  {
    console.log(`${this.name} moved ${distanceInMeters}m.`)
  }
}

class Snake extends Animal
{
  constructor(name : string)
  {
    super(name)
  }

  move(distanceInMeters = 5)
  {
    console.log("slithering..");
    super.move(distanceInMeters);
  }
}

class Horse extends Animal
{
  constructor(name : string)
  {
    super(name)
  }

  move(distanceInMeters = 75)
  {
    console.log("Galloping..");
    super.move(distanceInMeters);
  }
}

let sam = new Snake("Sam the python");
let Jammy : Animal = new Horse("Jammy the python");

sam.move();
Jammy.move(85); */
// Inheritance Finished
// Accessibility
/*  class Person {
  protected name: string;
  constructor(name: string) {
    this.name = name;
  }
}

class Employee extends Person {
  private department: string;

  constructor(name: string, department: string) {
    super(name);
    this.department = department;
  }

  public getElevatorPitch() {
    return `Hello, my name is ${this.name} and I work in ${this.department}.`;
  }
}

let howard = new Employee("Howard", "Sales");
console.log(howard.getElevatorPitch());
console.log(howard.name); // error */
// Optional Properties
/* interface SquareConfig {
  color?: string;
  width?: number;
}

function createSquare(config: SquareConfig): { color: string; area: number } {
  let newSquare = { color: "white", area: 100 };
  if (config.color) {
    newSquare.color = config.color;
  }
  if (config.width) {
    newSquare.area = config.width * config.width;
  }
  return newSquare;
}

let mySquare = createSquare({ color: "black" });
let myBigSquare = createSquare({ color: "blue", width: 500 });

console.log(mySquare);
console.log(myBigSquare); */
// Indexable Types
/* class Animal {
  name: string;
}
class Dog extends Animal {
  breed: string;
}

class Test{}
// Error: indexing with a numeric string might get you a completely separate type of Animal!
interface NotOkay {
  [x: number]: Animal;
  [x: string]: Dog;  // Replacing Dog to Test will solve the error
}

interface NumberDictionary {
  [index: string]: number;
  length: number; // ok, length is a number
  name: string; // error, the type of 'name' is not a subtype of the indexer
  // changing string to number solve the error
} */
// Accessors
/* const fullNameMaxLength = 10;

class Employee {
  private _fullName: string;

  get fullName(): string {
    return this._fullName;
  }

  set fullName(newName: string) {
    if (newName && newName.length > fullNameMaxLength) {
      throw new Error("fullName has a max length of " + fullNameMaxLength);
    }

    this._fullName = newName;
  }
}

let employee = new Employee();
employee.fullName = "Bob Smith";
if (employee.fullName) {
  console.log(employee.fullName);
} */
// Static Properties
/* class DemoCounter {
  static counter : number = 0;
  name : string = "";
  increament(name: string) : void {
      DemoCounter.counter++;
      this.name = name;
  }
  static doSoemthing(): void{
      console.log("printing from static method");
  }
}

let demoCounter : DemoCounter = new DemoCounter();
let demoCounter2 : DemoCounter = new DemoCounter();
demoCounter.increament("x");
demoCounter.increament("y");
demoCounter2.increament("z");
console.log(DemoCounter.counter);
console.log(demoCounter.name);
console.log(demoCounter2.name);
DemoCounter.doSoemthing(); */
// Abstract Classes
/* abstract class Department {
  constructor(public name: string) {}

  printName(): void {
    console.log("Department name: " + this.name);
  }

  abstract printMeeting(): void; // must be implemented in derived classes
}

class AccountingDepartment extends Department {
  constructor() {
    super("Accounting and Auditing"); // constructors in derived classes must call super()
  }

  printMeeting(): void {
    console.log("The Accounting Department meets each Monday at 10am.");
  }

  generateReports(): void {
    console.log("Generating accounting reports...");
  }
}

let department: Department; // ok to create a reference to an abstract type
department = new Department(); // error: cannot create an instance of an abstract class
department = new AccountingDepartment(); // ok to create and assign a non-abstract subclass
department.printName();
department.printMeeting();
department.generateReports(); // error: method doesn't exist on declared abstract type */
// Constructor functions
/* class Greeter {
  greeting: string;
  constructor(message: string) {
    this.greeting = message;
  }
  greet() {
    return "Hello, " + this.greeting;
  }
}

let greeter: Greeter;
greeter = new Greeter("world");
console.log(greeter.greet()); // "Hello, world" */
/* let Greeter = (function() {
  function Greeter(message) {
    this.greeting = message;
  }
  Greeter.prototype.greet = function() {
    return "Hello, " + this.greeting;
  };
  return Greeter;
})();

let greeter;
greeter = new Greeter("world");
console.log(greeter.greet()); // "Hello, world" */
// Generics
function Greeter(value) {
    return value;
}
var output = Greeter("hello");
console.log(output);
