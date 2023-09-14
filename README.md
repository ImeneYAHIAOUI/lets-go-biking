# Let's Go Biking Sourour GAZZEH - Imene YAHIAOUI SI4 FISA
## Nos Suppositions

Les informations sur les contrats sont conservées sur notre cache pendant un jour et une minute pour les stations.



L'idée générale c'est de calculer l'itinéraire le plus rapide de la maniére suivante :
![image](https://user-images.githubusercontent.com/79751594/205460069-fc8103e1-1e07-40f9-ac27-bbdbcfa2f031.png)
1. trouver la station s0 la plus proche de la position de départ avec des vélos disponibles
2. trouver la station s3 la plus proche de la destination avec des places de stationnement disponible
3. si s0 et s3 n'appartiennent pas au même contrat
   * trouver la station s1 qui appartient au même contrat que s0 et dont la somme des deux distances s0 -> s1 et s1 -> destination est la plus petite et qui a des vélos disponibles
   * trouver la station s2 qui appartient au même contrat que s3 et dont la distance s1 -> s2 est la plus petite et qui a des places de stationnement disponibles
4. le trajet que le client devrait faire à pied vers la station d'origine, entre les stations intermédiares, et vers la destination est pris en compte
 
  
Notre serveur renvoie l'itinéraire le plus court au client ( que ce soit en vélo ou à pied).

## Comment ça marche ?

Avant de lancer le client, il faut d'abord lancer le fichier exécutable du proxy (server/ProxyAndCache/bin/Release ) puis le fichier exécutable du serveur (server/SelfRootingServer/bin/Release) en tant qu'administrateur pour les deux et enfin démarrer l'active MQ.

Pour lancer le client il suffit de lancer le fichier App.java avec intellij.

En lançant le client une fenêtre s'affiche qui permet à l'utilisateur de saisir l'adresse du départ et l'adresse de la destination. 

![image](https://user-images.githubusercontent.com/79751594/205903027-1622e584-fcb8-40c5-be36-2bf4f6ea233c.png)
 
 pour obtenir les itinérairees il suffit de cliquer sur le bouton "Click to get itinerary".
 
 * Si le client ne saisie rien : 
 
    ![image](https://user-images.githubusercontent.com/79751594/205907479-09f84126-e253-4005-9c61-4159dd493767.png)

 * Si le client saisie une adresse invalide ou le serveur n'est pas connecté a internet : 
 
    On affiche un message pour demander à l'utilisateur de vérifier ce qu'il a saisie.
 
     ![image](https://user-images.githubusercontent.com/90778346/205986425-d9ff2741-e51a-4c00-921f-e12a1876405e.png)
 * Si le client saisie 2 adresses valides : 
 
     ![image](https://user-images.githubusercontent.com/79751594/205904122-6a08d55e-ad43-4faa-8820-ade8109d0fcc.png)
      
      Sur le terminal, Le client veut visualiser les instrutctions envoyées sur l'active mq ainsi que le nom de la queue.
      
     ![image](https://user-images.githubusercontent.com/79751594/205904588-c3694f5d-e723-495f-928f-2d01cf85b7fb.png)

      Pour demander de vérifier si les itinéraires sont à jours et d'envoyer ce qui reste, il faut cliquer sur le bouton "Update itinerary".
      
      En faisant ça, une fenêtre s'affiche pour demander à l'utilisateur de saisir sa position courante.
      
      ![image](https://user-images.githubusercontent.com/79751594/205909386-da183f0a-2b50-4cce-b8c8-eb657d7b7c07.png)

       
      En cliquant sur le bouton "Update", le client aura la réponse sur la map. Sur le terminal il aura une partie des itineraires qui ont été envoyés sur l'active    mq.
       
       
      ![image](https://user-images.githubusercontent.com/79751594/205906649-f4778ec0-b4c8-490f-bbb0-ccdaa5f826b0.png)
       


      ![image](https://user-images.githubusercontent.com/79751594/205906596-4efbe81b-c9f5-4ac5-a5ff-aa58358964da.png)

      

 
  
  

 
