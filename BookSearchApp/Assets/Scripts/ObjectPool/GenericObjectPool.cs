using System.Collections;
using System.Collections.Generic;

/* 
 
 * Usage 
 
 *  
 
 * GenericObjectPool<GameObject> monster_pool; 
 
 * ... 
 
 *  
 
 * // Create monsters. 
 
 * this.monster_pool = new GenericObjectPool<GameObject>(5, () =>  
 
    {  
 
        GameObject obj = new GameObject("monster"); 
 
        return obj; 
 
    }); 
 
     
 
     
 
    ... 
 
     
 
    // Get from pool 
 
    GameObject obj = this.monster_pool.pop(); 
 
     
 
    ... 
 
     
 
    // Return to pool 
 
    this.monster_pool.push(obj); 
 
 * */

public class GenericObjectPool<T> where T : class
{
    short m_Count;

    System.Func<T> m_Create;

    Stack<T> m_Objects;

    public GenericObjectPool(short count, System.Func<T> fn)
    {
        this.m_Count = count;

        this.m_Create = fn;

        this.m_Objects = new Stack<T>(this.m_Count);

        allocate();
    }

    void allocate()
    {
        for (int i = 0; i < this.m_Count; ++i)
        {
            m_Objects.Push(this.m_Create());
        }
    }

    public T pop()
    {
        if (this.m_Objects.Count <= 0)
        {
            allocate();
        }

        return this.m_Objects.Pop();
    }

    public void push(T obj)
    {
        this.m_Objects.Push(obj);
    }
}