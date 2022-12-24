using System.Collections;
using System.Collections.Generic;

/*==============================================
 * Author: Destiny Dahlgren
 * Description: This is a utility class that exists
 *      as a simple implementation of a First In
 *      Last Out queue system
 *==============================================*/

public class FILO<T>
{
    public int size = 0;
    private List<T> list = new List<T>();
    public FILO()
    { 
    }

    //Desc: Adds the given element to the end of the queue
    //Param: Takes the element to add
    //Return: None
    public void PushBack(T element)
    {
        list.Add(element);
        size++;
    }

    //Desc: Returns and removes the last thing that was added to the queue
    //Param: None
    //Return: <T> element
    public T Pop()
    {
        if (size > 0)
        {
            T element = list[size - 1];
            list.RemoveAt(size - 1);
            size--;
            return element;
        }
        else
        {
            return default(T);
        }
    }
}
