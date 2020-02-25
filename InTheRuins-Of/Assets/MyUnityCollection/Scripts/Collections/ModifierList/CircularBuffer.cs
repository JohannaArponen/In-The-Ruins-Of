using System.Collections.Generic;
using System;

public class CircularBuffer<T> {

  private T[] data;
  private CircularInt _head;



  protected Stack<T> list;


  public CircularBuffer(int length) {
    data = new T[length];
  }

  public void Pop() {
  }
}
