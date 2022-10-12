# Денис Карачун 053503
## 1. Тема: Интернет-касса с билетами
## 2. Функциональные требования к проекту:
### Роли пользователей:
1. Вокзал
2. Клиент
3. Администратор
### Авторизация пользователя
Пользователь должнен иметь возможность:
- Зарегистрироваться.
- Войти в аккаунт, выйти.
- Восстановить пароль
### Возможности пользователей
- Администратор
    + Создать, удалить, отредактировать вокзал.
    + Создать, просмотреть, отредактировать или удалить любого пользователя
    + Журналирование действий вокзалов
- Вокзал
    + Создать, просмотреть, отредактировать или удалить машинистов.
    + Создать, просмотреть, отредактировать, удалить билеты.
    + Создать, просмотреть, отредактировать, удалить поезда, изменить конфигурацию вагонов поезда.

- Клиент
    + Просмотреть, добавить в корзину, удалить из корзину билеты.
    + Просмотреть информацию о поезде билета (вагоны (их типы), машинисты)

## 3. Список сущностей БД:
- Покупатели
- Корзина
- Администратор
- Билеты
- Станции
- Поезд 
- Типы поезда
- Название типа
- Вагон
- Тип вагона
- Машинист
- Лог станции
- Лог покупателя

## 4. Схематично изобразить не нормальзованную схему БД:
![image](https://user-images.githubusercontent.com/79207007/195412994-1e905145-a2e2-4138-b961-43de045e2651.png)



## 5. Описать каждую сущность:
- Покупатели (связь с сущностью "корзина" типом связи one-to-one)

| Имя поля | Тип    | Описание  | Ограничения   |
|----------|---------|---------|---------------|
|CustomerID|INT(4) | Первичный ключ      | Обязательное поле, уникальный, авто-инкрементация             |
|Nickname          |VARCHAR(64)  | Никнейм покупателя      |Обязательное поле, уникальный               |
|City          |VARCHAR(32)   | Город пользователя, можно использовать для рекомендаций билетов из родного города     |Необязательное поле, запрет на ввод чисел                 |
|Email          |VARCHAR(64) | Почта, можно использовать для сброса пароля, рассылки рекламы и в качестве логина       |Обязательное поле, уникальная               |
|Name          |VARCHAR(32)  | Имя пользователя       |Обязательное поле, запрет на ввод чисел                |
|Surname          |VARCHAR(32)  | Фамилия пользоватея      |Обязательное поле, запрет на ввод чисел                  |
|Password          |VARCHAR(32)  | Пароль пользователя      |Обязательное поле                |

- Корзина (связь с сущностью "корзина" типом связи one-to-one и связь с сущность "Билет" типом связи one-to-many)

| Имя поля | Тип    | Описание  | Ограничения   |
|----------|---------|---------|---------------|
|CartID|INT(4) |Первичный ключ       | Обязательное поле, уникальный, авто-инкрементация             |
|TickedID          |INT(4)  | Внешний ключ к "Tickets" , для хранения заказанных билетов в корзине     |Обязательное поле               |
|Total price          |INT(16)    | Общая стоимость товаров в корзине     |Обязательное поле                 |

- Администратор

| Имя поля | Тип    | Описание  | Ограничения   |
|----------|---------|---------|---------------|
|AdminID|INT(4)   |Первичный ключ    |Обязательное поле, уникальный, авто-инкрементация             |
|Nickname          |VARCHAR(64) | Никнейм администратора, чтобы в будущем отслеживать какой именно администратор сделать гадость       |Обязательное поле, уникальный               |
|Email          |VARCHAR(64)  | Почта, она же выступает в качестве логина       |Обязательное поле, уникальная               |
|Password          |VARCHAR(32)  | Пароль администратора       |Обязательное поле                |


- Билеты (связь с сущностью "корзина" типом связи one-to-many, связь с сущностью "поезд" типом связи one-to-many, связь с сущностью "станции" типом связи one-to-many)


| Имя поля | Тип    | Описание  | Ограничения   |
|----------|---------|---------|---------------|
|TicketID|INT(4)  |Первичный ключ     | Обязательное поле, уникальный, авто-инкрементация |
|Starting point |VARCHAR(64) | Место отбытия     |Обязательное поле |
|Destination point |VARCHAR(64) | Место прибытия   |Обязательное поле |
|Price |INT(16) | Непосредственно цена билета  |Обязательное поле |
|TrainID |INT(4) | Внешний ключ к "Train" , для получения описания поезда  | Обязательное поле |
|Departure time |DATETIME | Время отправления   |Обязательное поле |
|Arriving time |DATETIME | Время прибытия  |Обязательное поле |
|IsBought |BOOL | Статус билета, заказанные билеты можно скрывать и удалять при оформлении покупки  |Обязательное поле |

- Станции (связь с сущностью "билеты" типом связи one-to-many)

| Имя поля | Тип    | Описание  | Ограничения   |
|----------|---------|---------|---------------|
|StationID |INT(4)  | Первичный ключ   | Обязательное поле, уникальный, авто-инкрементация |
|Station name |VARCHAR(64)  | Название станции |Обязательное поле |
|TicketID |INT(4) | Внешний ключ к "Tickets", каждая станция должна продавать определённые билеты  | Обязательное поле |
|Login |VARCHAR(32) | Логин станции |Обязательное поле |
|Password |VARCHAR(32) | Пароль  |Обязательное поле |
|Email |VARCHAR(64) | Почта станции, может использоваться администратором для рассылки спама  |Обязательное поле, уникальная |

- Поезд (связь с сущностью "машинисты" типом связи many-to-many, связь с сущностью "тип вагона" типом связи many-to-many, связь с сущностью "типы поезда" типом связи one-to-one)

| Имя поля | Тип    | Описание  | Ограничения   |
|----------|---------|---------|---------------|
|TrainID |INT(4)   | Первичный ключ   | Обязательное поле, уникальный, авто-инкрементация |
|MachinistID |INT(4) | Внешний ключ к "Machinists", для присвоения поезду определённых машинистов | Обязательное поле |
|TicketID |INT(4) | Внешний ключ к "Ticket", для присвоению поезду конкретных билетов | Обязательное поле |

- Машинисты (связь с сущностью "поезд" типом связи many-to-many)
 
| Имя поля | Тип    | Описание  | Ограничения   |
|----------|---------|---------|---------------|
|MachinistID |INT(4)   |  Первичный ключ  |Обязательное поле, уникальный, авто-инкрементация |
|TrainID |INT(4) | Внешний ключ к "Train", для присвоению машинистам конкретных поездов | Обязательное поле | 
|Name  |VARCHAR(32)    |  Имя машиниста   |Обязательное поле, запрет на ввод чисел  |
|Surname          |VARCHAR(32) | Фамилия машиниста  |Обязательное поле, запрет на ввод чисел  |

- Типы поезда (связь с сущностью "поезд" типом связи one-to-many, связь с сущностью "название типа" типом связи one-to-many)

| Имя поля | Тип    | Описание  | Ограничения   |
|----------|---------|---------|---------------|
|TrainTypesID |INT(4)|Первичный ключ | Обязательное поле, уникальный, авто-инкрементация |
|TrainID |INT(4) | Внешний ключ к "Train", для присвоения конкретному поезда определённого типа | Обязательное поле | 
|TypeID |INT(4) | Внешний ключ к "TypeName", для присвоения поезду конкретного типа | Обязательное поле | 

- Название типа (связь с сущностью "типы поезда" типом связи one-to-many)

| Имя поля | Тип    | Описание  | Ограничения   |
|----------|---------|---------|---------------|
|TypeID |INT(4)  | Первичный ключ    |Обязательное поле, уникальный, авто-инкрементация |
|Name |VARCHAR(64)  | Название типа поезда (например скоростной)   |Обязательное поле, уникальное | 

- Типы вагона (связь с сущностью "поезд" типом связи one-to-many, связь с сущностью "вагон" типом связи one-to-many)

| Имя поля | Тип    | Описание  | Ограничения   |
|----------|---------|---------|---------------|
|ID |INT(4)  | Первичный ключ    | Обязательное поле, уникальный, авто-инкрементация |
|TrainID |INT(4)  |Внешний ключ к "Train", для присвоения конкретному вагону роли |Обязательное поле | 
|CarriageID |INT(4) |Внешний ключ к "Carriage", для присвоения конкретной роли  | Обязательное поле |

- Вагон (связь с сущностью "типы вагона" типом связи one-to-many)

| Имя поля | Тип    | Описание  | Ограничения   |
|----------|---------|---------|---------------|
|CarriageID |INT(4)   | Первичный ключ   |Обязательное поле, уникальный, авто-инкрементация |
|CarriageName |VARCHAR(64) | Название вагона (его тип, например вагон-ресторан) |Обязательное поле, уникальное | 

- Лог станции 

| Имя поля | Тип    | Описание  | Ограничения   |
|----------|---------|---------|---------------|
|STLogID |INT(4)  | Первичный ключ    | Обязательное поле, уникальный, авто-инкрементация |
|StationID |INT(4)  |Внешний ключ к "Station", для просмотра кем было сделано изменение  |Обязательное поле | 
|LogType |VARCHAR(64) | Вывод типа действия |Обязательное поле | 
|LogMsg |VARCHAR(64) | Вывод информации о действии |Обязательное поле| 

- Лог пользователя 

| Имя поля | Тип    | Описание  | Ограничения   |
|----------|---------|---------|---------------|
|CMLogID |INT(4)  | Первичный ключ    | Обязательное поле, уникальный, авто-инкрементация |
|CustomerID |INT(4)  |Внешний ключ к "Customer", для просмотра кем было сделано изменение  |Обязательное поле | 
|LogType |VARCHAR(64) | Вывод типа действия |Обязательное поле | 
|LogMsg |VARCHAR(64) | Вывод информации о действии |Обязательное поле|
