# Desafio
A empresa Battle Royalle Solutions está com a demanda para controlar máquinas Windows remotamente usando CLI.
A empresa então decidiu que a melhor solução para eles, seria a criação de uma aplicação cliente que seria executado como um Windows Service, e uma aplicação Web para prover a interface que permitiria executar os comandos em uma ou várias máquinas.

Usando a aplicação Web, deve ser possível escolher uma máquina para interagir com ela executando comandos e recebendo os resultados dele, ou seja, ter um terminal na aplicação web.

# Requisitos

- Deve ser possível executar comandos como “dir, cd” do cmd e comandos do powershell também e receber o retorno deles.
- A aplicação cliente deve se registrar na aplicação web, tornando possível a interação com aquela máquina.
- A aplicação web não deve deixar executar comandos em maquinas que não estão com o serviço cliente em execução(possível motivo: máquina desligada).
- Deve ser possível registrar o log de execução de comandos bem como o retorno deles, para serem visualizados posteriormente.
- Deve ser possível executar um mesmo comando em várias máquinas de uma única vez.
- A aplicação cliente deve enviar os seguintes dados da máquina local no momento do registro:
- Nome da máquina
- IP Local
- Antivirus instalado
- Firewall está ativo
- Versão do Windows
- Versão do .NET Framework instalado
- Tamanho dos HDs (disponível e total)
- A aplicação cliente deve ter um instalador fácil.
- O instalador da aplicação cliente deve permitir a instalação sem interface gráfica(instalação quiet).

# Avaliação

Os seguintes pontos serão considerados para uma boa avaliação do seu projeto.

- Arquitetura da aplicação
- Código limpo
- Respeito as boas práticas de programação como KISS, YAGNI, SOLID e etc
- Utilização de padrões de projeto
- Utilização de um framework SPA para o Front-End
- Automatização de build/deploy
- Uso de testes unitários
- Utilização de tecnologias recentes, bem como a versão mais nova do C# / .NET Core 3.1
- Utilização de recursos de integração com GitHub.

# O projeto (SocketIO)

O projeto foi desenvolvido utilizando arquitetura de comunicação via websockets. As tecnologias utilizadas foram:

- Plataforma .NET Core 3.1
- Topshelf 4.2.1
- Angular 10.0.9
- xUnit

Os projeto foi dividos em:

- SocketIO.Server: Gerenciador mestre de conexões
- SocketIO.PackageManager: Gerenciador de pacotes (conversões)
- SocketIO.Service: Aplicação para ser implantada (como um serviço) em máquinas clientes
- SocketIO.SPA: Aplicação web para enviar/receber comandos das máquinas clientes

### Instalação do serviço

Copiar o serviço para o diretório desejado e executar os seguintes comandos (como administrador):

```
$ \install\directory\SocketIO.Service.exe install 
$ \install\directory\SocketIO.Service.exe start
```

### Utilização

O servidor (SocketIO.Server) rodará na porta 5001 e ficará intermediando os pacotes entre as aplicações.
O cliente (SocketIO.SPA) rodará na porta 3001, onde será possível realizar o gerenciamento das máquinas 
(onde devem possuir o serviço instalado e sendo executado).

A interface cliente, quando recebe uma nova conexão, exibe um card com os dados da máquina, um input para
introduzir os comandos (na barra superior, ao lado direito, existe um input para enviar um comando a todas
as máquinas) e um textarea logo abaixo para a exibição da saída. Os comandos seguem o padrão:

```
(cmd | ps | log) args
```

- cmd: Prompt de comando
- ps: Powershell
- log: Recurso para recuperação de logs

Quando um comando é enviado para um cliente, o mesmo registra o comando solicitado e sua saída, estes podem
ser recuperados da seguinte maneira:

```
log first => retorna o primeiro log registrado 
log last => retorna o último log registrado
log => retorna todos os registros
```