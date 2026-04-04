#!/bin/bash
# Script para configurar e rodar o projeto MatuTI

echo "=== Configurando MatuTI ==="

cd MatuTI

echo "1. Restaurando pacotes..."
dotnet restore

echo "2. Criando migration inicial..."
dotnet ef migrations add Inicial

echo "3. Aplicando migration..."
dotnet ef database update

echo "4. Iniciando aplicação..."
dotnet run

echo "=== Acesse: https://localhost:5001 ==="
echo "Login padrão: admin@matuti.com / Admin@123"
