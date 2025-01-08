using System.Collections.Generic;
using UnityEngine;

public class MurderGameManager : MonoBehaviour
{
    [System.Serializable]
    public class Clue
    {
        public string name; // Namn på ledtråden
        public bool isReal; // Är ledtråden riktig?
        public GameObject prefab; // Prefab för ledtråden
    }

    [System.Serializable]
    public class Character
    {
        public string name; // Namn på karaktären
        public bool isMurderer; // Är denna karaktär mördare?
        public GameObject characterSprite; // Karaktärens sprite
        public GameObject deadSprite; // Karaktärens döda sprite
        public List<Clue> characterClues = new List<Clue>(); // Karaktärens ledtrådar
    }

    public List<Transform> spawnPoints; // Platser där ledtrådar kan spawnas
    public List<Character> characters; // Lista över karaktärer i spelet
    public List<Clue> chosenClues = new List<Clue>(); // Valda ledtrådar för spelet

    private void Start()
    {
        StartGame();
    }

    public void StartGame()
    {
        ChooseMurderer(); // Välj mördare
        ChooseVictim();   // Välj offer
        ChooseClues();    // Välj ledtrådar
        SpawnClues();     // Spawn ledtrådar på kartan
    }

    void ChooseMurderer()
    {
        // Slumpa en mördare
        int randomIndex = Random.Range(0, characters.Count);
        Character murderer = characters[randomIndex];
        murderer.isMurderer = true;

        // Välj 2 riktiga ledtrådar och 1 fejk ledtråd
        List<Clue> realClues = murderer.characterClues.FindAll(clue => clue.isReal);
        List<Clue> fakeClues = murderer.characterClues.FindAll(clue => !clue.isReal);

        if (realClues.Count < 2 || fakeClues.Count < 1)
        {
            Debug.LogError("Inte tillräckligt med ledtrådar för att ge mördaren!");
            return;
        }

        // Lägg till 2 riktiga ledtrådar
        for (int i = 0; i < 2; i++)
        {
            int randomRealIndex = Random.Range(0, realClues.Count);
            chosenClues.Add(realClues[randomRealIndex]);
            realClues.RemoveAt(randomRealIndex);
        }

        // Lägg till 1 fejk ledtråd
        int randomFakeIndex = Random.Range(0, fakeClues.Count);
        chosenClues.Add(fakeClues[randomFakeIndex]);

        Debug.Log("Mördaren är: " + murderer.name);
    }

    void ChooseVictim()
    {
        // Skapa en lista över alla karaktärer som inte är mördaren
        List<Character> possibleVictims = characters.FindAll(character => !character.isMurderer);

        // Kontrollera att det finns minst en karaktär som kan vara offer
        if (possibleVictims.Count == 0)
        {
            Debug.LogError("Det finns inga karaktärer som kan vara offer!");
            return;
        }

        // Välj slumpmässigt en karaktär från listan
        int randomIndex = Random.Range(0, possibleVictims.Count);
        Character victim = possibleVictims[randomIndex];

        // Hantera offrets död
        Debug.Log("Offret är: " + victim.name);
        HandleDeath(victim);
    }

    void HandleDeath(Character victim)
    {
       // Inaktivera karaktärens sprite för att visa att de är döda
       if (victim.characterSprite != null)
       {
           victim.characterSprite.SetActive(false);
       }

       // Aktivera offrets döda sprite om det finns en
       if (victim.deadSprite != null)
       {
           victim.deadSprite.SetActive(true);
       }

       Debug.Log("Offret " + victim.name + " är nu död.");
    }

    void ChooseClues()
    {
        // Alla ledtrådar är redan valda av mördaren
        Debug.Log("Valda ledtrådar: " + string.Join(", ", chosenClues.ConvertAll(c => c.name)));
    }

    void SpawnClues()
    {
        if (spawnPoints.Count < chosenClues.Count)
        {
            Debug.LogError("Det finns inte tillräckligt många spawn points för alla ledtrådar!");
            return;
        }

        // Shuffle spawn points
        List<Transform> shuffledSpawnPoints = new List<Transform>(spawnPoints);
        for (int i = 0; i < shuffledSpawnPoints.Count; i++)
        {
            int randomIndex = Random.Range(0, shuffledSpawnPoints.Count);
            Transform temp = shuffledSpawnPoints[i];
            shuffledSpawnPoints[i] = shuffledSpawnPoints[randomIndex];
            shuffledSpawnPoints[randomIndex] = temp;
        }

        // Spawn each clue at a random spawn point
        for (int i = 0; i < chosenClues.Count; i++)
        {
            Clue clue = chosenClues[i];
            Transform spawnPoint = shuffledSpawnPoints[i];

            // Instantiate the clue at the spawn point
            GameObject clueInstance = Instantiate(clue.prefab, spawnPoint.position, Quaternion.identity);
            clueInstance.SetActive(true); // Aktiverar den valda ledtråden
        }

        Debug.Log("Ledtrådar har spawnats på kartan!");
    }
}