# Desafio Microsserviço SaldoArch

### Iniciar

docker-compose -f docker-compose.yml -f docker-compose.override.yml up -d

### Duvidas/Erro/Problemas

- Fiquei na duvida quanto ao Motor de Estado (Porem a API de Transacao conseguiu executar conforme solicitado)
- Ultilização do Retry não sei se apliquei certo, porem executou conforme solicitado
- Não consegui fazer/finalizar a APIGateway

### Aprendizados

- Criação de um Microserviço
- Ultilização do Mongodb
- Ultilização do Redis
- Ultilização do MemoryCache da aplicação
- Ultilização do Retry/Policy
- Outros

### REALIZAR TRANSAÇÃO

HTTP POST URL -> http://localhost:8000/swagger/index.html

### CONSULTAR SALDO

HTTP GET URL -> http://localhost:8081/swagger/index.html

