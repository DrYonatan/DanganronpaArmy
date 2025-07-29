using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;

public class EvidenceManager : MonoBehaviour
{
    [SerializeField] UIBullet selectedBullet;
    List<Evidence> evidences;
    [SerializeField] private RectTransform cylinder;
    int selectedIndex;

    private void Update()
    {
        if(Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            selectedIndex++;
            GameLoop.instance.debateUIAnimator.LoadBullet();
            cylinder.DOLocalRotate(cylinder.localRotation.eulerAngles + new Vector3(0, 0, 60), 0.4f);
            if (selectedIndex >= evidences.Count)
            {
                selectedIndex = 0;
            }
            UpdateEvidence();
        }

        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            GameLoop.instance.debateUIAnimator.LoadBullet();
            cylinder.DOLocalRotate(cylinder.localRotation.eulerAngles + new Vector3(0, 0, -60), 0.4f);
            selectedIndex--;
            if(selectedIndex < 0)
            {
                selectedIndex = evidences.Count-1;
            }
            UpdateEvidence();
        }
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
}
