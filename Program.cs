var source = Enumerable.Range(1,5);

var acoll = source.Select(i=>new A(i));
A[] arr1 = new A[5];
A[] arr2 = new A[5];
int i=0;
/* on énumère 2 fois */
foreach(var item in acoll){
    arr1[i++]=item;
}
i=0;
foreach(var item in acoll){
    arr2[i++]=item;
}
/* on énumère et compare */
for(var j=0; j<5; j++){
    if(arr1[j]==arr2[j]) {
        Console.WriteLine($"{arr1[j]} == {arr2[j+5]}");
    } else {
        Console.WriteLine($"{arr1[j]} != {arr2[j+5]}");
    }
}




class A {
    public int i {get;}
    public A(int i)
    {
        this.i=i;
    }
    public override string ToString() =>$"A({i}) (#{GetHashCode()})";
}