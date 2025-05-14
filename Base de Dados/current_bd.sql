use onbiscato;
select * from usuario;
select * from solicitacoes;
select * from servico;
select * from chat;
INSERT INTO `chat` (`solicitacao_id`, `remetente_id`, `mensagem`)
VALUES (20, 5, 'Olá, como posso ajudar?');
ALTER TABLE solicitacoes ADD imagem LONGBLOB;
Update usuario set nome = "Delvio Correia" where id = 11;
