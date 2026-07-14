♟️ Xadrez em Realidade Aumentada (AR Chess)

Projeto Realidade Aumentada da discilpina Computação Gráfica para o curso de Ciência da Computação no Instituto Federal de Sergipe (IFS).

Este aplicativo para Android utiliza tecnologia de Realidade Aumentada (AR) para projetar um tabuleiro de xadrez 3D interativo em qualquer superfície plana do mundo real. O objetivo é proporcionar uma experiência imersiva e moderna do clássico jogo de tabuleiro, fundindo o ambiente físico com elementos digitais.
🛠️ Tecnologias Utilizadas

    Motor Gráfico: Unity 3D

    Linguagem: C#

    Realidade Aumentada: AR Foundation & Google ARCore

    Plataforma Alvo: Android

✨ Funcionalidades

O jogo foi construído com um sistema de regras e interações feitas do zero, focando em uma experiência de multiplayer local (pass-and-play):

    Mapeamento de Superfície: Utiliza a câmera do smartphone para detectar superfícies físicas (como mesas) e ancorar o tabuleiro de xadrez em escala real.

    Sistema de Turnos Local: Alternância automática entre a vez das peças Brancas e Pretas. Apenas o jogador do turno atual pode interagir com suas respectivas peças.

    Validação Matemática de Movimento: O motor do jogo calcula a distância física do toque do jogador e converte para "casas" da grade (0.0625m por casa), validando os padrões clássicos de movimento:

        Cavalo: Movimento em "L" preciso.

        Peão: Movimento reto, com restrição de captura exclusiva nas diagonais.

        Torre, Bispo e Rainha: Movimentação em linhas retas ou diagonais ilimitadas.

        Rei: Movimentação restrita a uma casa em qualquer direção.

    Radar Anti-Pulo (Detecção de Caminho): Sistema robusto de verificação de física (OverlapSphere) que impede que as peças atravessem umas às outras, bloqueando jogadas ilegais (com a devida exceção para o salto do Cavalo).

    Sistema de Captura e Condição de Vitória: Peças inimigas são destruídas ao serem capturadas. O jogo identifica automaticamente a captura do Rei inimigo, trava a tela para impedir novos movimentos e declara a equipe vencedora na interface.

    Interface (UI) Imersiva: Feedback visual tátil (peças selecionadas brilham em amarelo) e interface limpa projetada para não obstruir a visão da câmera (Glassmorphism).

📱 Como Jogar

    Início: Ao abrir o aplicativo, aponte a câmera para uma superfície bem iluminada e texturizada.

    Posicionamento: Toque na tela onde deseja que o tabuleiro apareça.

    A Partida: Pressione o botão Start para destravar as peças.

    Interação: * Toque na sua peça (ela ficará amarela indicando a seleção).

        Toque na casa de destino ou na peça inimiga que deseja capturar.

        Passe o celular para o adversário realizar a jogada dele.

    Fim de Jogo: O jogo termina automaticamente quando um dos Reis é capturado.

👨‍💻 Autor

Talyson Estudante de Ciência da Computação - IFS (Instituto Federal de Sergipe)
