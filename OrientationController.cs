using UnityEngine;

public class OrientationController : MonoBehaviour {

    [Header("Orientation Settings")]
    [SerializeField] private GameObject model;
    [SerializeField] private bool startsWest = true;
    private bool facingWest;

    public bool FacingWest {
        get { return facingWest; }
        private set { facingWest = value; }
    }

    public GameObject Model {
        get { return model; }
        private set { model = value; }
    }

    protected virtual void Start() {

        if (model == null) {
            Debug.LogError("There is no model assigned in the Inspector for a Object that uses it!");
        }

        if (!startsWest) {
            model.transform.Rotate(0, 180, 0);
        }

        facingWest = true;
    }

    public void UpdateOrientation(float dirX) {

        if (dirX > 0) {
            ResetModel();
        }
        else if (dirX < 0) {
            FlipModel();
        }
    }

    public void ResetModel() {

        if (model != null) {

            if (facingWest) {
                return;
            }

            model.transform.localScale = new Vector3(1, 1, 1);;
            facingWest = true;
        }
    }

    public void FlipModel() {

        if (model != null) {

            if (!facingWest) {
                return;
            }

            model.transform.localScale = new Vector3(-1, 1, 1);
            facingWest = false;
        }
    }
}
