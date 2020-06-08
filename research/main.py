import numpy as np
from sklearn.linear_model import LinearRegression
from sklearn.linear_model import Ridge
from sklearn.preprocessing import PolynomialFeatures
from sklearn.pipeline import make_pipeline

import matplotlib.pyplot as plt

x = np.array([10000, 20000, 30000, 40000, 50000, 60000, 70000, 80000, 90000, 100000, 110000, 120000, 130000, 140000, 150000, 160000, 170000, 180000, 190000, 200000])
y1 = np.array([
3793.50,
13584.99,
30598.15,
63322.49,
118520.30,
173918.10,
258604.75,
316780.45,
434325.70,
683140.70,
690995.95,
1139076.70,
2468583.70,
2873720.40,
3207533.00,
3692480.00,
4115183.05,
4541076.10,
5022309.70,
5808948.15,
])
y2 = np.array([
55.42,
110.46,
219.87,
233.18,
287.83,
340.39,
403.74,
470.23,
564.02,
829.26,
902.39,
965.08,
725.27,
1115.71,
1223.36,
1265.64,
936.42,
983.23,
1563.19,
1564.01,
])

x1 = x.reshape((-1,1))
x2 = x.reshape((-1,1))

model1 = LinearRegression().fit(x1, y1)
model2 = LinearRegression().fit(x2, y2)

score1 = model1.score(x1, y1)
score2 = model1.score(x2, y2)

colors = ['teal', 'yellowgreen', 'gold']
lw=2

#old algorithm
plt.scatter(x, y1, color='navy', s=30, marker='o', label="Old")
for count, degree in enumerate([1, 2]):
    model = make_pipeline(PolynomialFeatures(degree), Ridge())
    model.fit(x1, y1)
    y_plot = model.predict(x1)
    plt.plot(x, y_plot, color=colors[count], linewidth=lw, label="degree %d" % degree)
plt.show()


#new algorithm
plt.scatter(x, y2, color='navy', s=30, marker='o', label="new")
for count, degree in enumerate([1, 2]):
    model = make_pipeline(PolynomialFeatures(degree), Ridge())
    model.fit(x2, y2)
    y_plot = model.predict(x2)
    plt.plot(x, y_plot, color=colors[count], linewidth=lw, label="degree %d" % degree)
plt.show()
