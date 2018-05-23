library(sqldf)
#library(sem)

setwd('C:\\Users\\User\\Source\\Repos\\General\\Cars\\AutotraderScraper\\AutotraderScraper\\bin\\Debug')
file = 'cars.csv'

#load main dataset
cars1 <- read.csv.sql(file, sql = 'select * from file ', header =TRUE, sep=",")

lm1 <- lm(formula = price ~ age + mileage, data = cars1)
