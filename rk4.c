#include<stdio.h>

//const FILE *fp=fopen("ex.dat","w");

double func(double x)
{
    
    return x;
    
}

void tworder(double x_0, double h, double t_0, double t_n)
{
    
    double n=(t_n-t_0)/h;
    double t=t_0, x=x_0;
    double k1, k2, k3, k4;
    int i;
    
    printf("%f\t%f\n", t, x);
//    fprintf(fp,"%f\t%f\n", t, x);
    
    for(i=1;i<=n;i++)
    {
        
        k1=func(x);
        k2=func(x+k1*h/2);
        k3=func(x+k2*h/2);
        k4=func(x+k3*h);
        x=x+(k1+2*k2+2*k3+k4)*h/6;
        t=t+h;
        
        printf("%f\t%f\n", t, x);
//        fprintf(fp,"%f\t%f\n", t, x);
        
    }

}

int main()
{   
    
    double h=0.01;
    double x_0=1;
    double t_0=0, t_n=10;
    tworder(x_0, h, t_0, t_n);
    //fclose(fp);
    return 0;

}