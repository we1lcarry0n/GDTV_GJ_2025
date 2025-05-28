using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class UpgradesController : MonoBehaviour
{
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private CinemachineCamera _upgradeCamera;
    [SerializeField] private GameObject _upgradeMenuUI;
    [SerializeField] private GameObject _powersSelectionScreen;
    [SerializeField] private List<Transform> _slotsToVisualize;
    [SerializeField] private Animator _playerAnimator;

    [SerializeField] private GameObject[] _powersToChooseRandomly;

    private int _selectedSlotNumber;
    private PowerVisual[] _equippedPowers = new PowerVisual[6];
    [SerializeField] private Button[] _equippedPowersButtons;

    private int _attemptNumber = 0;

    private int firstTakenPowerIndex = -1;
    private int secondTakenPowerIndex = -1;
    private int thirdTakenPowerIndex = -1;

    public void InitializeUpgradeMenu()
    {
        if (_attemptNumber == 0)
        {
            _equippedPowersButtons[0].gameObject.SetActive(true);
        }
        if (_attemptNumber == 1)
        {
            _equippedPowersButtons[1].gameObject.SetActive(true);
        }
        if (_attemptNumber == 2)
        {
            _equippedPowersButtons[2].gameObject.SetActive(true);
        }
        _upgradeCamera.Priority = 2;
        _upgradeMenuUI.SetActive(true);
        StartCoroutine(MenuShowRoutine());
    }

    public void CloseUpgradeMenu()
    {
        _attemptNumber++;
        _playerController.SetMovement(true);
        _upgradeCamera.Priority = 0;
        _upgradeMenuUI.SetActive(false);
        _playerAnimator.SetTrigger("cocoonExit");
    }

    public void OpenPowerSelection(int slotNumber)
    {
        _selectedSlotNumber = slotNumber;
        _powersSelectionScreen.SetActive(true);
        GenerateThreeRandom();
    }

    private void GenerateThreeRandom()
    {
        int firstIndex = 0;
        int secondIndex = 0;
        int thirdIndex = 0;
        while (firstIndex == firstTakenPowerIndex || firstIndex == secondTakenPowerIndex ||  firstIndex == thirdTakenPowerIndex)
        {
            firstIndex = Random.Range(0, _powersToChooseRandomly.Length - 1);
        }
        secondIndex = Random.Range(0, _powersToChooseRandomly.Length - 1);
        while (secondIndex == firstIndex || secondIndex == firstTakenPowerIndex || secondIndex == secondTakenPowerIndex || secondIndex == thirdTakenPowerIndex)
        {
            secondIndex = Random.Range(0, _powersToChooseRandomly.Length - 1);
        }
        thirdIndex = Random.Range(0, _powersToChooseRandomly.Length - 1);
        while (thirdIndex == firstIndex || thirdIndex == secondIndex || thirdIndex == firstTakenPowerIndex || thirdIndex == secondTakenPowerIndex || thirdIndex == thirdTakenPowerIndex)
        {
            thirdIndex = Random.Range(0, _powersToChooseRandomly.Length - 1);
        }
        _powersToChooseRandomly[firstIndex].SetActive(true);
        _powersToChooseRandomly[secondIndex].SetActive(true);
        _powersToChooseRandomly[thirdIndex].SetActive(true);
    }

    public void ClosePowerSelection()
    {
        foreach(GameObject power in _powersToChooseRandomly)
        {
            power.SetActive(false);
        }
        _powersSelectionScreen.SetActive(false);
    }

    public void ApplyUpgradeInCurrentSlot(PowerVisual powerVisual, GameObject upgradeVisual)
    {
        if (_attemptNumber == 0)
        {
            firstTakenPowerIndex = powerVisual.upgradeIndex;
        }
        if (_attemptNumber == 1)
        {
            secondTakenPowerIndex = powerVisual.upgradeIndex;
        }
        if (_attemptNumber == 2)
        {
            thirdTakenPowerIndex = powerVisual.upgradeIndex;
        }
        Instantiate(upgradeVisual, _slotsToVisualize[_selectedSlotNumber]);
        _equippedPowers[_selectedSlotNumber] = powerVisual;
        _equippedPowersButtons[_selectedSlotNumber].interactable = false;
        ClosePowerSelection();
    }

    public void ClearUpgradeInCurrentSlot()
    {
        if (_slotsToVisualize[_selectedSlotNumber].childCount != 0)
        {
            GameObject toDestroy = _slotsToVisualize[_selectedSlotNumber].GetChild(0).gameObject;
            Destroy(toDestroy);
        }
        if (_equippedPowers[_selectedSlotNumber] != null)
        {
            _equippedPowers[_selectedSlotNumber].MarkUnequipped();
        }
        _equippedPowers[_selectedSlotNumber] = null;
    }

    private IEnumerator MenuShowRoutine()
    {
        yield return new WaitForSeconds(1.5f);
        _playerAnimator.SetTrigger("cocoonEnter");
    }
}
