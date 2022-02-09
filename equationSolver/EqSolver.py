import csv
from itertools import count
import numpy as np
import matplotlib.pyplot as plt
from scipy import interpolate

x = []
y = []
with open('data.csv', newline='') as csvfile:
    data = csv.reader(csvfile, delimiter=' ', quotechar='|')
    count = 0
    for row in data:
        [xx, yy] = row[0].split(",")
        count += 1
        if count % 1 == 0:
            x.append(float(xx))
            y.append(float(yy))

func = interpolate.splrep(x, y, s=0)
xnew = 0.1
ynew = interpolate.splev(xnew, func, der=0)


def function(xIn):
    y = interpolate.splev(xIn, func, der=0)
    return y


def df(xIn):
    if xIn == 0:
        dx = 0.0001
    else:
        dx = xIn * 0.02
    x_l = xIn - dx / 2
    x_r = xIn + dx / 2
    y_l = function(x_l)
    y_r = function(x_r)
    dx = x_r - x_l
    dy = y_r - y_l
    dydx = dy / dx
    return dydx


dy = []
for xIn in x:
    dy.append(df(xIn) / 100)

xintpl = np.linspace(x[0], x[-1], 100)
yintpl = []
for xIn in xintpl:
    yintpl.append(function(xIn))

plt.figure()
plt.plot(x, y, "x")
plt.plot(x, dy)
plt.plot(xintpl, yintpl)

x_cur = 0.1
iterFlag = True
count = 0
while (count < 1000) and iterFlag:
    x_next = x_cur - function(x_cur) / df(x_cur)
    x_cur = x_next
    count += 1
    if function(x_next) < 1e-5 and function(x_next) > -1e-5:
        iterFlag = False
    plt.plot(x_next, function(x_next), 's')

print(x_next, function(x_next))
print('count', count)

plt.legend(['sample points', 'dydx', 'cubic spline', 'point'])
# plt.axis([-0.05, 0.3, -1.05, 1.05])
plt.title('Cubic-spline interpolation')
plt.show()
