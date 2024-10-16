using System;
using Server;
using Server.Gumps;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Services.Virtues;

namespace Server.Items
{
    public class VirtueDistillationFlask : Item
    {
        [Constructable]
        public VirtueDistillationFlask() : base(0x1832)
        {
            Name = "Virtue Distillation Flask";
            Weight = 1.0;
        }

        public VirtueDistillationFlask(Serial serial) : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (from is PlayerMobile player)
            {
                // Check if player is alive and not mounted
                if (!player.Alive)
                {
                    player.SendMessage("You cannot use this while dead.");
                    return;
                }

                from.SendGump(new VirtueDistillationGump(player, this));
            }
        }

        public void DistillVirtue(PlayerMobile from, VirtueName virtue)
        {
            if (VirtueHelper.HasAny(from, virtue))
            {
                int currentVirtuePoints = from.Virtues.GetValue((int)virtue);
                
                if (currentVirtuePoints >= 5000)
                {
                    // Reduce virtue points by 5000
                    VirtueHelper.Atrophy(from, virtue, 5000);

                    // Create corresponding virtue stone item
                    Item virtueStone = GetVirtueStoneItem(virtue);

                    if (virtueStone != null)
                    {
                        from.AddToBackpack(virtueStone);
                        from.SendMessage("You have distilled a " + virtueStone.Name + " from your virtue.");
                    }
                }
                else
                {
                    from.SendMessage("You do not have enough points in this virtue to distill.");
                }
            }
            else
            {
                from.SendMessage("You do not have any points in this virtue.");
            }
        }

        private Item GetVirtueStoneItem(VirtueName virtue)
        {
            switch (virtue)
            {
                case VirtueName.Compassion:
                    return new CompassionStone();
                case VirtueName.Honesty:
                    return new HonestyStone();
                case VirtueName.Honor:
                    return new HonorStone();
                case VirtueName.Justice:
                    return new JusticeStone();
                case VirtueName.Sacrifice:
                    return new SacrificeStone();
                case VirtueName.Spirituality:
                    return new SpiritualityStone();
                case VirtueName.Valor:
                    return new ValorStone();
                case VirtueName.Humility:
                    return new HumilityStone();
                default:
                    return null;
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class VirtueDistillationGump : Gump
    {
        private readonly PlayerMobile m_Player;
        private readonly VirtueDistillationFlask m_Flask;

        public VirtueDistillationGump(PlayerMobile player, VirtueDistillationFlask flask) : base(50, 50)
        {
            m_Player = player;
            m_Flask = flask;

            AddBackground(0, 0, 300, 400, 5054);
            AddLabel(100, 20, 1152, "Select a Virtue to Distill");

            int y = 60;

            foreach (VirtueName virtue in VirtueHelper.Virtues)
            {
                AddButton(50, y, 4005, 4007, (int)virtue + 1, GumpButtonType.Reply, 0);
                AddLabel(90, y, 1152, virtue.ToString());
                y += 40;
            }
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            if (m_Player == null || m_Flask == null || m_Flask.Deleted)
                return;

            int buttonID = info.ButtonID - 1;

            if (buttonID >= 0 && buttonID < VirtueHelper.Virtues.Length)
            {
                VirtueName selectedVirtue = VirtueHelper.Virtues[buttonID];
                m_Flask.DistillVirtue(m_Player, selectedVirtue);
            }
        }
    }
}
