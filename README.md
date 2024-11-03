# DATABASE MIAGRATION FOR POS-SYSTEM

This is the db migration set-up for the POS-System project of the Windows Programming class.

## To run the set-up

### Node and PostgreSQL

Make sure you have **Node** and **you can create a PostgreSQL database and access, query, interact with it**.
If you don't, you can checkout this [link](https://tdquang7.notion.site/T-o-migration-cho-database-v-i-docker-v-node-12d8139672a080cabe27d29f5da82c7f) (of Mr.Tran Duy Quang) to install Node (and Docker if you want).
_Also, there should be **default database "postgres"** to be connected to initially._

**Note**: In the provided link, the docker command was used to make a `mssql` server, this project requires a `postgresql` server. So after installing Docker, you can run this line instead:

```
$ docker run -e "POSTGRES_PASSWORD=..." -e "POSTGRES_USER=..." -e "POSTGRES_DB=POSSystem" -p 5432:5432 --name postgres_db_POS_system -d postgres
```

where `...` means something you type in (your database credential), the host is `localhost` by default, `5432` is the default port for `postgresql`. If your username, password is `user123`, `pass123`, then the line should be:

```
$ docker run -e "POSTGRES_PASSWORD=pass123" -e "POSTGRES_USER=user123" -e "POSTGRES_DB=POSSystem" -p 5432:5432 --name postgres_db_POS_system -d postgres
```

### `knex` in CLI

Ensure you can run `knex` as a command in your CLI (Command Prompts, PowerShell, Bash...) like `knex migrate:latest`.
This can be achieved if you have installed knex globally (and also inside this project), please refer to [this](https://gist.github.com/NigelEarle/80150ff1c50031e59b872baf0e474977) for more information.

### Configure the .env file

The host, port, database name should be received from the project owners.
<br>However, the **username**, **password** should be your own database credential, just make sure that yours have enough privilages so that the app can perform CRUD operations, login using Google Auth, and use Stripe API.
