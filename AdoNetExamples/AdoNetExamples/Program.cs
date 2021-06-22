using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AdoNetExamples
{
    public class Program
    {
        static void Main(string[] args)
        {
            var house1 = new House
            {
                Id = 1,
                Room = new List<Room>{
                    new Room
                {Id=1,Rent=255,Door=new Door
                {Id=1,Height=9.5,Camera=new Camera
                {Id=1,CameraPX=13}},Window=new Window
                {Id=1,size=55.5}},
                    new Room
                {Id=2,Rent=785,Door=new Door
                {Id=2,Height=10.5,Camera=new Camera
                {Id=2,CameraPX=17}},Window=new Window
                {Id=2,size=75.5}}}
            };

            var house2 = new House
            {
                Id = 2,
                Room = new List<Room>{
                    new Room
                {Id=3,Rent=7895,Door=new Door
                {Id=3,Height=8.5,Camera=new Camera
                {Id=3,CameraPX=83}},Window=new Window
                {Id=3,size=98.5}},
                    new Room
                {Id=4,Rent=695,Door=new Door
                {Id=4,Height=3.5,Camera=new Camera
                {Id=4,CameraPX=29}},Window=new Window
                {Id=4,size=17.5}}}
            };

            var updatedHouse2 = new House
            {
                Id = 2,
                Room = new List<Room>{
                    new Room
                {Id=3,Rent= 888,Door=new Door
                {Id=3,Height= 99.5,Camera=new Camera
                {Id=3,CameraPX=11}},Window=new Window
                {Id=3,size=55.5}},
                    new Room
                {Id=4,Rent=222,Door=new Door
                {Id=4,Height=66.5,Camera=new Camera
                {Id=4,CameraPX=22}},Window=new Window
                {Id=4,size=77.5}}}
            };

            var myorm = new MyORM<House>("Server=DESKTOP-9BILDI2\\SQLEXPRESS;Database=aspnetB5;User Id = aspnetb5; Password=hidden;");

            myorm.Insert(house1);
            myorm.Insert(house2);
            myorm.Update(updatedHouse2);
            House house = myorm.GetById(1);
            IList<House> houses = myorm.GetAll();
            myorm.Delete(1);
            myorm.Delete(house2);

            // All Done Alhamdulillah
        }



    }
}
