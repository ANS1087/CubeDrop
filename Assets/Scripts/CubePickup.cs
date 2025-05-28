using UnityEngine;
using Unity.Netcode;

public class CubePickup : NetworkBehaviour
{
    public Transform holdPoint;
    private GameObject heldObject;
    public float pickupRange = 2f;
    public KeyCode interactKey = KeyCode.E;

    void Update()
    {
        if (!IsOwner) return;

        if (Input.GetKeyDown(interactKey))
        {
            if (heldObject == null)
            {
                TryPickupCube();
            }
            else
            {
                DropCube();
            }
        }

        if (heldObject != null)
        {
            heldObject.transform.position = holdPoint.position;
            heldObject.transform.rotation = holdPoint.rotation;
        }
    }

    void TryPickupCube()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, pickupRange);

        foreach (var hit in hits)
        {
            if (hit.CompareTag("redcube") || hit.CompareTag("greencube") || hit.CompareTag("bluecube") || hit.CompareTag("yellowcube"))
            {
                var rb = hit.attachedRigidbody;
                if (rb == null) continue;

                GameObject cube = hit.gameObject;
                var netObj = cube.GetComponent<NetworkObject>();
                if (netObj == null) continue;

                // 🟡 Ask the server to give ownership
                RequestOwnershipServerRpc(netObj.NetworkObjectId);

                heldObject = cube;

                rb.useGravity = false;
                rb.isKinematic = true;

                break;
            }
        }
    }

    void DropCube()
    {
        if (heldObject == null) return;

        var rb = heldObject.GetComponent<Rigidbody>();
        rb.useGravity = true;
        rb.isKinematic = false;

        heldObject = null;
    }

    // ✅ ServerRpc to transfer ownership
    [ServerRpc]
    void RequestOwnershipServerRpc(ulong objectId)
    {
        if (NetworkManager.SpawnManager.SpawnedObjects.TryGetValue(objectId, out NetworkObject netObj))
        {
            netObj.ChangeOwnership(OwnerClientId);
        }
    }
}
