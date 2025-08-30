#!/bin/bash

# Inicia o SQL Server em segundo plano
/opt/mssql/bin/sqlservr &

# Aguarda o SQL Server estar disponível
echo "Aguardando o SQL Server ficar disponível..."
until /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "MgFinanceiroSenha123Forte" -C -Q "SELECT 1" > /dev/null 2>&1; do
  sleep 1
  echo "Ainda aguardando o SQL Server ficar disponível..."
done

# Verifica se o banco MgFinanceiroDb já existe
echo "Verificando se o banco MgFinanceiroDb existe..."
DB_CHECK=$(/opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "MgFinanceiroSenha123Forte" -C -Q "SET NOCOUNT ON; IF DB_ID('MgFinanceiroDb') IS NOT NULL SELECT 1 ELSE SELECT 0" -h -1 -W)
if [ $? -ne 0 ]; then
  echo "Erro ao executar a consulta de verificação do banco de dados."
  exit 1
fi

# Remove espaços em branco e verifica o resultado
DB_EXISTS=$(echo "$DB_CHECK" | tr -d '[:space:]')
echo "Resultado da verificação do banco: '$DB_EXISTS'"

if [ "$DB_EXISTS" = "1" ]; then
  echo "MgFinanceiroDb já existe, pulando execução do init.sql."
else
  echo "MgFinanceiroDb não existe, executando init.sql..."
  /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "MgFinanceiroSenha123Forte" -C -i /db/init.sql
  if [ $? -eq 0 ]; then
    echo "init.sql executado com sucesso."
  else
    echo "Erro ao executar init.sql."
    exit 1
  fi
fi

# Mantem o container ativo
wait