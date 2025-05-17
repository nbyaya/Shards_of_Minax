using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Targeting;
using Server.Network;
using Server.Spells;
using Server.Misc;

namespace Server.Mobiles
{
    [CorpseName("a bonebound marksman corpse")]
    public class BoneboundMarksman : BaseCreature
    {
        private DateTime m_NextGhostArrow;
        private DateTime m_NextCurseVolley;
        private DateTime m_NextPhaseStep;
        private bool m_AbilitiesInitialized;

        [Constructable]
        public BoneboundMarksman()
            : base(AIType.AI_Archer, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a Bonebound Marksman";
            Body = 0x8E; // Same as Ratman Archer
            Hue = 2101; // Pale spectral blue
            BaseSoundID = 437;

            SetStr(300, 350);
            SetDex(160, 200);
            SetInt(150, 180);

            SetHits(700, 850);

            SetDamage(14, 22);
            SetDamageType(ResistanceType.Physical, 60);
            SetDamageType(ResistanceType.Energy, 40);

            SetResistance(ResistanceType.Physical, 55, 70);
            SetResistance(ResistanceType.Fire, 40, 55);
            SetResistance(ResistanceType.Cold, 50, 65);
            SetResistance(ResistanceType.Poison, 60, 75);
            SetResistance(ResistanceType.Energy, 60, 80);

            SetSkill(SkillName.Archery, 110.0, 120.0);
            SetSkill(SkillName.Tactics, 90.0, 100.0);
            SetSkill(SkillName.MagicResist, 80.0, 100.0);
            SetSkill(SkillName.SpiritSpeak, 85.0, 100.0);
            SetSkill(SkillName.Wrestling, 70.0, 85.0);

            Fame = 18000;
            Karma = -18000;

            VirtualArmor = 60;

            AddItem(new Bow { Hue = 1150 });
            PackItem(new Arrow(Utility.RandomMinMax(100, 150)));
        }

        public override bool AutoDispel => true;
        public override bool CanRummageCorpses => true;
        public override int TreasureMapLevel => 4;

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    m_NextGhostArrow = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(5, 10));
                    m_NextCurseVolley = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 20));
                    m_NextPhaseStep = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 30));
                    m_AbilitiesInitialized = true;
                }

                if (DateTime.UtcNow >= m_NextGhostArrow)
                    GhostArrow();

                if (DateTime.UtcNow >= m_NextCurseVolley)
                    CurseVolley();

                if (DateTime.UtcNow >= m_NextPhaseStep)
                    PhaseStep();
            }
        }

        private void GhostArrow()
        {
            PublicOverheadMessage(MessageType.Regular, 0x482, false, "*Draws and fires a spectral arrow...*");

            if (Combatant is Mobile target)
            {
                target.PlaySound(0x22F); // Ghostly sound
                Effects.SendMovingEffect(this, target, 0xF42, 10, 0, false, false); // Ethereal arrow effect
                Timer.DelayCall(TimeSpan.FromMilliseconds(500), () =>
                {
                    if (!Deleted && target.Alive)
                    {
                        AOS.Damage(target, this, Utility.RandomMinMax(25, 40), 0, 0, 0, 0, 100);
                        target.SendMessage(38, "A ghostly arrow sears through your soul!");
                        target.Mana -= Utility.RandomMinMax(5, 15);
                    }
                });
            }

            m_NextGhostArrow = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
        }

        private void CurseVolley()
        {
            PublicOverheadMessage(MessageType.Regular, 0x22, false, "*Unleashes a volley of cursed arrows!*");
            PlaySound(0x234);

            IPooledEnumerable e = GetMobilesInRange(6);
            foreach (Mobile m in e)
            {
                if (m != this && m.Alive && m is PlayerMobile)
                {
                    Effects.SendMovingEffect(this, m, 0xF42, 7, 0, false, false); // Volley effect
                    Timer.DelayCall(TimeSpan.FromMilliseconds(500), () =>
                    {
                        if (!Deleted && m.Alive)
                        {
                            AOS.Damage(m, this, Utility.RandomMinMax(15, 25), 0, 100, 0, 0, 0);
                            m.SendMessage(44, "You are struck by a cursed arrow!");
                            m.ApplyPoison(this, Poison.Regular);
                        }
                    });
                }
            }
            e.Free();

            m_NextCurseVolley = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
        }

        private void PhaseStep()
        {
            PublicOverheadMessage(MessageType.Regular, 0x9C2, false, "*Fades from one place to another...*");
            PlaySound(0x20F);

            Map map = Map;
            if (map == null || map == Map.Internal)
                return;

            for (int i = 0; i < 10; i++)
            {
                int x = X + Utility.RandomMinMax(-4, 4);
                int y = Y + Utility.RandomMinMax(-4, 4);
                int z = map.GetAverageZ(x, y);

                if (map.CanSpawnMobile(x, y, z))
                {
                    Location = new Point3D(x, y, z);
                    Effects.SendLocationEffect(Location, Map, 0x3728, 10, 1, 0, 0);
                    break;
                }
            }

            m_NextPhaseStep = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 40));
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich);
            AddLoot(LootPack.Gems, 4);
            AddLoot(LootPack.FilthyRich);

            if (Utility.RandomDouble() < 0.01) // 1% chance
            {
                PackItem(new BoneboundCowl()); // A rare loot item
            }
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            if (Utility.RandomDouble() < 0.03)
            {
                c.DropItem(new SpectralArrowRelic());
            }
        }

        public BoneboundMarksman(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            reader.ReadInt();
            m_AbilitiesInitialized = false;
        }
    }

    public class BoneboundCowl : HoodedShroudOfShadows
    {
        [Constructable]
        public BoneboundCowl()
        {
            Name = "Bonebound Cowl";
            Hue = 2101;
            Attributes.NightSight = 1;
            Attributes.Luck = 150;
        }

        public BoneboundCowl(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            reader.ReadInt();
        }
    }

    public class SpectralArrowRelic : Item
    {
        [Constructable]
        public SpectralArrowRelic() : base(0x1BD3) // Arrow
        {
            Hue = 1150;
            Name = "Spectral Arrow Relic";
            LootType = LootType.Blessed;
            Weight = 1.0;
        }

        public SpectralArrowRelic(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            reader.ReadInt();
        }
    }
}
