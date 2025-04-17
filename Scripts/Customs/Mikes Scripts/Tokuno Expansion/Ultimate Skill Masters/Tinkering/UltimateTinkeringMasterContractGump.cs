using System;
using Server;
using Server.Gumps;
using Server.Network;
using Server.Mobiles;
using Server.Items;
using Server.Targeting;

namespace Server.Gumps
{
    public class UltimateTinkeringMasterContractGump : Gump
    {
        private UltimateTinkeringMasterContract m_Contract;

        public UltimateTinkeringMasterContractGump(Mobile from, UltimateTinkeringMasterContract contract) : base(0, 0)
        {
            m_Contract = contract;

            AddPage(0);
            AddBackground(0, 0, 300, 200, 5170);
            AddLabel(40, 40, 0, $"Collect {contract.AmountNeeded} {contract.ItemType.Name}(s)");
            AddLabel(40, 60, 0, $"Collected: {contract.AmountCollected}");

            AddHtml(40, 80, 220, 60, "Return all items to me. Once complete, claim your reward.", false, false);

            if (contract.AmountCollected < contract.AmountNeeded)
            {
                AddButton(90, 140, 4005, 4007, 1, GumpButtonType.Reply, 0);
                AddLabel(125, 140, 0, "Add Item");
            }
            else
            {
                AddButton(90, 140, 4005, 4007, 2, GumpButtonType.Reply, 0);
                AddLabel(125, 140, 0, "Claim Reward");
            }
        }

        public override void OnResponse(NetState state, RelayInfo info)
        {
            Mobile from = state.Mobile;

            if (info.ButtonID == 1) // Add Item
            {
                from.SendMessage("Target the required item in your backpack.");
                from.Target = new UltimateTinkeringItemTarget(m_Contract);
            }
            else if (info.ButtonID == 2) // Claim Reward
            {
                if (m_Contract.AmountCollected >= m_Contract.AmountNeeded)
                {
                    from.SendMessage("Your rewards have been placed in your backpack!");

                    // Give Gold (optional)
                    from.AddToBackpack(new Gold(m_Contract.AmountNeeded * 500));

                    // Determine player's current Tinkering skill cap
                    double currentCap = 100.0;
                    Skill Tinkering = from.Skills[SkillName.Tinkering];
                    if (Tinkering != null)
                        currentCap = Tinkering.Cap;

                    double newCap = currentCap + 10.0;
                    if (newCap > 150.0)
                        newCap = 150.0;

                    if (newCap > currentCap)
                    {
                        // Drop Tinkering Powerscroll
                        PowerScroll ps = new PowerScroll(SkillName.Tinkering, newCap);
                        from.AddToBackpack(ps);
                    }
                    else
                    {
                        from.SendMessage("Your Tinkering skill is already at or above the maximum (150). No skill scroll given.");
                    }

                    // Drop a special random reward
                    Item specialReward = UltimateTinkeringMasterReward.GetRandomReward();
                    if (specialReward != null)
                        from.AddToBackpack(specialReward);

                    m_Contract.Delete();
                }
                else
                {
                    from.SendMessage("You have not collected all the required items yet.");
                }
            }
        }
    }

    public class UltimateTinkeringItemTarget : Target
    {
        private UltimateTinkeringMasterContract m_Contract;

        public UltimateTinkeringItemTarget(UltimateTinkeringMasterContract contract) : base(1, false, TargetFlags.None)
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

                if (m_Contract.AmountCollected >= m_Contract.AmountNeeded)
                {
                    from.SendMessage("All required items collected! You can now claim your reward.");
                }
                else
                {
                    from.PrivateOverheadMessage(
                                MessageType.Regular,
                                0x3B2,
                                false,
                               $"You have added the items to the contract. {m_Contract.AmountCollected}/{m_Contract.AmountNeeded} collected.",
                                from.NetState
                            );
                    from.PrivateOverheadMessage(
                                MessageType.Regular,
                                0x3B2,
                                false,
                               $"Target the next item.",
                                from.NetState
                            );
                    from.Target = new UltimateTinkeringItemTarget(m_Contract); // Reinitialize the targeting
                }
            }
            else
            {
                from.SendMessage("That item is either not the one required or not in your backpack.");
            }
        }
    }
}
