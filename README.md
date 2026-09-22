# UnityCyberBallGame

Juego de acción en Unity. El jugador controla a **Roboto**: anda, salta, puede pasar a un modo bola más rápido y tiene un dash. El nivel tiene enemigos que disparan, puertas, botones y edificios que se pueden romper.

El repositorio versiona la carpeta `Assets` (scripts y el contenido que cuelga de ella), no un paquete listo para publicar.

## Scripts propios

```
Assets/Scripts/
├── Player/          RobotoController, PlayerInputManager
├── Hability/        Dash y habilidades
├── Enemies/         Warrior, disparos y pool de balas
├── Environment/     Door, Button, Building y destrucción de mallas
└── Systems/         Health, Damage, Destroy, ObjectPool
```

`RobotoController` usa `CharacterController`. Separa velocidad de pie (`moveSpeed`) y velocidad en modo bola (`ballSpeed`), aplica gravedad y un salto.

## Stack

- Unity
- C#

## Cómo abrirlo

1. Crea un proyecto Unity vacío o abre uno existente.
2. Copia `Assets` de este repo encima del `Assets` del proyecto (o clona y añade un `ProjectSettings` desde Unity Hub con **Add project from disk** si la carpeta ya lo trae en tu copia local).
3. Abre la escena de juego y comprueba que el jugador tiene `RobotoController` y `PlayerInputManager`.

Si al clonar solo ves `Assets`, Unity Hub puede crear los ajustes de proyecto al abrirlo la primera vez. Elige una versión LTS reciente de Unity 6 o 2022, la misma con la que se guardaron las escenas si aparecen avisos de upgrade.
