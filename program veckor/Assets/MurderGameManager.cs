using System.Collections.Generic;
using UnityEngine;

public class MurderGameManager : MonoBehaviour
{
    [System.Serializable]
    public class Clue
    {
        public string name; // Namn p� ledtr�den
        public bool isReal; // �r ledtr�den riktig?
        public GameObject prefab; // Prefab f�r ledtr�den
    }

    [System.Serializable]
    public class Character
    {
        public string name; // Namn p� karakt�ren
        public bool isMurderer; // �r denna karakt�r m�rdare?
        public GameObject characterSprite; // Karakt�rens sprite
        public GameObject deadSprite; // Karakt�rens d�da sprite
        public List<Clue> characterClues = new List<Clue>(); // Karakt�rens ledtr�dar
    }

    public List<Transform> spawnPoints; // Platser d�r ledtr�dar kan spawnas
    public List<Character> characters; // Lista �ver karakt�rer i spelet
    public List<Clue> chosenClues = new List<Clue>(); // Valda ledtr�dar f�r spelet

    private void Start()
    {
        StartGame();
    }

    public void StartGame()
    {
        ChooseMurderer(); // V�lj m�rdare
        ChooseVictim();   // V�lj offer
        ChooseClues();    // V�lj ledtr�dar
        SpawnClues();     // Spawn ledtr�dar p� kartan
    }

    void ChooseMurderer()
    {
        // Slumpa en m�rdare
        int randomIndex = Random.Range(0, characters.Count);
        Character murderer = characters[randomIndex];
        murderer.isMurderer = true;

        // V�lj 2 riktiga ledtr�dar och 1 fejk ledtr�d
        List<Clue> realClues = murderer.characterClues.FindAll(clue => clue.isReal);
        List<Clue> fakeClues = murderer.characterClues.FindAll(clue => !clue.isReal);

        if (realClues.Count < 2 || fakeClues.Count < 1)
        {
            Debug.LogError("Inte tillr�ckligt med ledtr�dar f�r att ge m�rdaren!");
            return;
        }

        // L�gg till 2 riktiga ledtr�dar
        for (int i = 0; i < 2; i++)
        {
            int randomRealIndex = Random.Range(0, realClues.Count);
            chosenClues.Add(realClues[randomRealIndex]);
            realClues.RemoveAt(randomRealIndex);
        }

        // L�gg till 1 fejk ledtr�d
        int randomFakeIndex = Random.Range(0, fakeClues.Count);
        chosenClues.Add(fakeClues[randomFakeIndex]);

        Debug.Log("M�rdaren �r: " + murderer.name);
    }

    void ChooseVictim()
    {
        // Skapa en lista �ver alla karakt�rer som inte �r m�rdaren
        List<Character> possibleVictims = characters.FindAll(character => !character.isMurderer);

        // Kontrollera att det finns minst en karakt�r som kan vara offer
        if (possibleVictims.Count == 0)
        {
            Debug.LogError("Det finns inga karakt�rer som kan vara offer!");
            return;
        }

        // V�lj slumpm�ssigt en karakt�r fr�n listan
        int randomIndex = Random.Range(0, possibleVictims.Count);
        Character victim = possibleVictims[randomIndex];

        // Hantera offrets d�d
        Debug.Log("Offret �r: " + victim.name);
        HandleDeath(victim);
    }

    void HandleDeath(Character victim)
    {
       // Inaktivera karakt�rens sprite f�r att visa att de �r d�da
       if (victim.characterSprite != null)
       {
           victim.characterSprite.SetActive(false);
       }

       // Aktivera offrets d�da sprite om det finns en
       if (victim.deadSprite != null)
       {
           victim.deadSprite.SetActive(true);
       }

       Debug.Log("Offret " + victim.name + " �r nu d�d.");
    }

    void ChooseClues()
    {
        // Alla ledtr�dar �r redan valda av m�rdaren
        Debug.Log("Valda ledtr�dar: " + string.Join(", ", chosenClues.ConvertAll(c => c.name)));
    }

    void SpawnClues()
    {
        if (spawnPoints.Count < chosenClues.Count)
        {
            Debug.LogError("Det finns inte tillr�ckligt m�nga spawn points f�r alla ledtr�dar!");
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
            clueInstance.SetActive(true); // Aktiverar den valda ledtr�den
        }

        Debug.Log("Ledtr�dar har spawnats p� kartan!");
    }
}