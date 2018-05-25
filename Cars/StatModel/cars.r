library(sqldf)
#library(sem)

setwd('C:\\Users\\User\\Source\\Repos\\General\\Cars\\AutotraderScraper\\AutotraderScraper\\bin\\Debug')
file = 'cars.csv'

#load main dataset
carsAll <- read.csv.sql(file, sql = 'select * from file ', header =TRUE, sep=",")
carsAuto <- read.csv.sql(file, sql = 'select * from file where transmission = "Automatic"', header =TRUE, sep=",")


lmAll <- lm(formula = price ~ age + mileage, data = carsAll)
lmAuto <- lm(formula = price ~ age + mileage, data = carsAuto)

summary(lmAll)
summary(lmAuto)
