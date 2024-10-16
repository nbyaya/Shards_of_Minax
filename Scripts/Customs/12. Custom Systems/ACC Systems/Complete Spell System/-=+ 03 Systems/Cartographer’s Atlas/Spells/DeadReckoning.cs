using System;
using Server;
using Server.Spells;
using Server.Network;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;


namespace Server.ACC.CSS.Systems.CartographyMagic
{
    public class DeadReckoning : CartographySpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Dead Reckoning",
            "Fel Xi Dor",
            21004,
            9300
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fifth; }
        }

        // CastDelay now returns a double, representing delay in seconds
        public override double CastDelay { get { return 0.2; } }

        public override double RequiredSkill => 80.0;
        public override int RequiredMana => 50;

        public DeadReckoning(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Caster.CloseGump(typeof(DeadReckoningGump));
                Caster.SendGump(new DeadReckoningGump(this));
            }
            else
            {
                FinishSequence();
            }
        }

        public void Teleport(int x, int y, int z)
        {
            Mobile m = Caster;
            Map map = m.Map;

            if (map == null || map == Map.Internal)
            {
                m.SendLocalizedMessage(1005569); // You can not teleport to that location.
                return;
            }

            Point3D loc = new Point3D(x, y, z);

            // Check if the location is valid
            if (!map.CanSpawnMobile(loc.X, loc.Y, loc.Z))
            {
                m.SendLocalizedMessage(1005569); // You can not teleport to that location.
                return;
            }

            // Visual effects
            m.PlaySound(0x1FE);
            Effects.SendLocationParticles(EffectItem.Create(m.Location, m.Map, EffectItem.DefaultDuration), 0x3728, 10, 10, 2023);
            Effects.SendLocationParticles(EffectItem.Create(loc, m.Map, EffectItem.DefaultDuration), 0x3728, 10, 10, 5023);

            m.MoveToWorld(loc, map);

            // More visual effects after teleportation
            m.PlaySound(0x1FE);
            Effects.SendLocationParticles(EffectItem.Create(m.Location, m.Map, EffectItem.DefaultDuration), 0x3728, 10, 10, 5023);

            // Sparkle effect
            for (int i = 0; i < 5; i++)
            {
                Timer.DelayCall(TimeSpan.FromMilliseconds(100 * i), () =>
                {
                    Effects.SendLocationParticles(EffectItem.Create(m.Location, m.Map, EffectItem.DefaultDuration), 0x376A, 9, 20, 5042);
                });
            }

            FinishSequence();
        }
    }

    public class DeadReckoningGump : Gump
    {
        private DeadReckoning m_Spell;

        public DeadReckoningGump(DeadReckoning spell) : base(50, 50)
        {
            m_Spell = spell;

            AddPage(0);
            AddBackground(0, 0, 300, 180, 9270);
            AddAlphaRegion(10, 10, 280, 160);

            AddHtml(10, 10, 280, 20, "<CENTER>Dead Reckoning</CENTER>", false, false);

            AddLabel(20, 40, 0x480, "X Coordinate:");
            AddBackground(120, 40, 150, 20, 9350);
            AddTextEntry(120, 40, 150, 20, 0, 1, "");

            AddLabel(20, 70, 0x480, "Y Coordinate:");
            AddBackground(120, 70, 150, 20, 9350);
            AddTextEntry(120, 70, 150, 20, 0, 2, "");

            AddLabel(20, 100, 0x480, "Z Coordinate:");
            AddBackground(120, 100, 150, 20, 9350);
            AddTextEntry(120, 100, 150, 20, 0, 3, "");

            AddButton(100, 140, 4005, 4007, 1, GumpButtonType.Reply, 0);
            AddHtml(135, 140, 100, 20, "Teleport", false, false);
        }

        public override void OnResponse(NetState state, RelayInfo info)
        {
            Mobile from = state.Mobile;

            if (info.ButtonID == 1)
            {
                string xText = info.GetTextEntry(1).Text;
                string yText = info.GetTextEntry(2).Text;
                string zText = info.GetTextEntry(3).Text;

                if (int.TryParse(xText, out int x) && int.TryParse(yText, out int y) && int.TryParse(zText, out int z))
                {
                    m_Spell.Teleport(x, y, z);
                }
                else
                {
                    from.SendMessage("Invalid coordinates. Please enter numbers only.");
                    from.SendGump(new DeadReckoningGump(m_Spell));
                }
            }
            else
            {
                m_Spell.FinishSequence();
            }
        }
    }
}
