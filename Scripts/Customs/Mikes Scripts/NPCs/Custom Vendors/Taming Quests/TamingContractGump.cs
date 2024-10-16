using System;
using Server;
using Server.Gumps;
using Server.Mobiles;
using Server.Items;
using Server.Network;
using Server.Targeting;

namespace Server.Gumps
{
    public class TamingContractGump : Gump
    {
        private TamingContract m_Contract;

        public TamingContractGump(Mobile from, TamingContract contract) : base(50, 50)
        {
            m_Contract = contract;

            AddPage(0);
            AddBackground(0, 0, 350, 250, 5170);
            AddLabel(75, 20, 0, "Taming Contract");
            AddLabel(50, 50, 0, $"Creatures to Tame: {contract.AmountToTame}");
            AddLabel(50, 70, 0, $"Creatures Tamed: {contract.AmountTamed}");
            AddLabel(50, 90, 0, $"Gold Reward: {contract.GoldReward}");
            AddLabel(50, 110, 0, $"Powerscroll: {contract.PowerScrollSkill} {contract.PowerScrollValue:F1}");
            AddLabel(50, 130, 0, "MaxxiaScroll");

            if (contract.AmountTamed < contract.AmountToTame)
            {
                AddButton(50, 170, 4005, 4007, 1, GumpButtonType.Reply, 0);
                AddLabel(85, 170, 0, "Turn In Tamed Creature");
            }
            else
            {
                AddButton(50, 170, 4005, 4007, 2, GumpButtonType.Reply, 0);
                AddLabel(85, 170, 0, "Claim Reward");
            }
        }

        public override void OnResponse(NetState state, RelayInfo info)
        {
            Mobile from = state.Mobile;

            if (info.ButtonID == 1)
            {
                from.SendMessage("Select the tamed creature to turn in.");
                from.Target = new TamingCreatureTarget(m_Contract);
            }
            else if (info.ButtonID == 2)
            {
                from.SendMessage("Your rewards have been deposited in your bank.");
                from.BankBox.DropItem(new BankCheck(m_Contract.GoldReward));
                
                PowerScroll ps = new PowerScroll(m_Contract.PowerScrollSkill, m_Contract.PowerScrollValue);
                from.BankBox.DropItem(ps);

                MaxxiaScroll ms = new MaxxiaScroll();
                from.BankBox.DropItem(ms);

                m_Contract.Delete();
            }
        }
    }

    public class TamingCreatureTarget : Target
    {
        private TamingContract m_Contract;

        public TamingCreatureTarget(TamingContract contract) : base(10, false, TargetFlags.None)
        {
            m_Contract = contract;
        }

        protected override void OnTarget(Mobile from, object targeted)
        {
            if (targeted is BaseCreature creature)
            {
                if (creature.Controlled && creature.ControlMaster == from && creature.GetType() == AnimalContractType.Get[m_Contract.CreatureType].Type)
                {
                    from.SendMessage("You have turned in the creature.");
                    m_Contract.AmountTamed++;
                    creature.Delete();

                    if (m_Contract.AmountTamed >= m_Contract.AmountToTame)
                    {
                        from.SendMessage("You have completed the taming contract!");
                        from.SendGump(new TamingContractGump(from, m_Contract));
                    }
                }
                else
                {
                    from.SendMessage("This creature does not meet the contract requirements.");
                }
            }
            else
            {
                from.SendMessage("That is not a valid tamed creature.");
            }
        }
    }
}