using System;
using Server;
using Server.Gumps;
using Server.Network;
using Server.Mobiles;
using Server.Items;
using Server.Targeting;

namespace Server.Gumps
{
    public class ItemCollectionContractGump : Gump
    {
        private ItemCollectionContract m_Contract;

        public ItemCollectionContractGump(Mobile from, ItemCollectionContract contract) : base(0, 0)
        {
            m_Contract = contract;

            AddPage(0);
            AddBackground(0, 0, 300, 200, 5170);
            AddLabel(40, 40, 0, $"Contract: Collect {contract.AmountNeeded} {contract.ItemType.Name}s");
            AddLabel(40, 60, 0, $"Collected: {contract.AmountCollected}");
            AddLabel(40, 80, 0, $"Powerscroll: {contract.PowerScrollSkill} {contract.PowerScrollValue:F1}");
            AddLabel(40, 100, 0, "MaxxiaScroll");

            if (contract.AmountCollected < contract.AmountNeeded)
            {
                AddButton(90, 140, 4005, 4007, 1, GumpButtonType.Reply, 0);
                AddLabel(104, 175, 0, "Add Item");
            }
            else
            {
                AddButton(90, 140, 4005, 4007, 2, GumpButtonType.Reply, 0);
                AddLabel(104, 175, 0, "Claim Reward");
            }
        }

        public override void OnResponse(NetState state, RelayInfo info)
        {
            Mobile from = state.Mobile;

            if (info.ButtonID == 1)
            {
                from.SendMessage("Target the item to add.");
                from.Target = new ItemTarget(m_Contract);
            }
            else if (info.ButtonID == 2)
            {
                from.SendMessage("Your rewards have been placed in your bank!");

                // Drop Gold Reward
                from.BankBox.DropItem(new Gold(m_Contract.AmountNeeded * 10)); 

                // Drop Powerscroll Reward
                PowerScroll ps = new PowerScroll(m_Contract.PowerScrollSkill, m_Contract.PowerScrollValue);
                from.BankBox.DropItem(ps);

                // Drop MaxxiaScroll
                MaxxiaScroll ms = new MaxxiaScroll();
                from.BankBox.DropItem(ms);

                m_Contract.Delete();
            }
        }
    }

    public class ItemTarget : Target
    {
        private ItemCollectionContract m_Contract;

        public ItemTarget(ItemCollectionContract contract) : base(1, false, TargetFlags.None)
        {
            m_Contract = contract;
        }

        protected override void OnTarget(Mobile from, object targeted)
        {
            if (targeted is Item item && item.GetType() == m_Contract.ItemType && item.IsChildOf(from.Backpack))
            {
                int itemAmount = item.Stackable ? item.Amount : 1;

                if (m_Contract.AmountCollected + itemAmount > m_Contract.AmountNeeded)
                {
                    int amountToAdd = m_Contract.AmountNeeded - m_Contract.AmountCollected;
                    m_Contract.AmountCollected = m_Contract.AmountNeeded;
                    item.Amount -= amountToAdd;

                    if (item.Amount <= 0)
                        item.Delete();
                }
                else
                {
                    m_Contract.AmountCollected += itemAmount;
                    item.Delete();
                }

                from.SendMessage("Item added to the contract.");

                if (m_Contract.AmountCollected >= m_Contract.AmountNeeded)
                {
                    from.SendMessage("You have collected all required items! Claim your reward.");
                }
            }
            else
            {
                from.SendMessage("That item is not part of this contract or is not in your backpack.");
            }
        }
    }
}
