using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;

public class EvidenceManager : MonoBehaviour
{
    [SerializeField] UIBullet selectedBullet;
    List<Evidence> evidences;
    [SerializeField] private RectTransform cylinder;
    [SerializeField] private BulletSelectionMenu bulletSelectionMenu;
    public int selectedIndex;

    public void LoadBullets()
    {
        GameLoop.instance.debateUIAnimator.LoadBullets(evidences);
    }

    void UpdateEvidence()
    {
        selectedBullet.text.text = this.evidences[selectedIndex].Name;
    }

    public void ShowEvidence(Evidence[] evidences)
    {
        this.evidences = evidences.ToList();
        this.evidences.RemoveAll(x => x == null);

        UpdateEvidence();
    }

    internal bool Check(Evidence correctEvidence)
    {
        return evidences[selectedIndex] == correctEvidence;
    }

    public void ShootBullet()
    {
        selectedBullet.Shoot();
    }

    public string GetSelectedEvidence()
    {
        return this.evidences[selectedIndex].Name;
    }

    public void SelectNextEvidence()
    {
        selectedIndex++;

        if (selectedIndex >= evidences.Count)
        {
            selectedIndex = 0;
        }

        if (bulletSelectionMenu.isOpen)
        {
            bulletSelectionMenu.UpdateBullets();
        }
        else
        {
            GameLoop.instance.debateUIAnimator.LoadBullet();
            cylinder.DOLocalRotate(cylinder.localRotation.eulerAngles + new Vector3(0, 0, 60), 0.4f);
        }

        UpdateEvidence();
    }

    public void SelectPreviousEvidence()
    {
        selectedIndex--;

        if (selectedIndex < 0)
        {
            selectedIndex = evidences.Count - 1;
        }
        
        if (bulletSelectionMenu.isOpen)
        {
            bulletSelectionMenu.UpdateBullets();
        }
        else
        {
            GameLoop.instance.debateUIAnimator.LoadBullet();
            cylinder.DOLocalRotate(cylinder.localRotation.eulerAngles + new Vector3(0, 0, -60), 0.4f);
        }
        
        UpdateEvidence();
    }
}