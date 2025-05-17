using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Items;
using Server.Spells;
using Server.Targeting;
using Server.Misc;

namespace Server.Mobiles
{
    [CorpseName("a sepulchral archmage's corpse")]
    public class SepulchralArchmage : BaseCreature
    {
        private DateTime m_NextPhaseShift;
        private DateTime m_NextSoulTorrent;
        private DateTime m_NextWailOfTheVeil;

        [Constructable]
        public SepulchralArchmage() 
            : base(AIType.AI_Mage, FightMode.Closest, 12, 1, 0.2, 0.4)
        {
            Name = "Sepulchral Archmage";
            Body = Utility.RandomList(125, 126);
            BaseSoundID = 0x423;
            Hue = 2755; // Eerie ghostly purple-green

            SetStr(300, 400);
            SetDex(90, 120);
            SetInt(800, 950);

            SetHits(2000, 2500);

            SetDamage(15, 25);
            SetDamageType(ResistanceType.Energy, 40);
            SetDamageType(ResistanceType.Cold, 30);
            SetDamageType(ResistanceType.Physical, 30);

            SetResistance(ResistanceType.Physical, 50, 65);
            SetResistance(ResistanceType.Fire, 40, 55);
            SetResistance(ResistanceType.Cold, 60, 75);
            SetResistance(ResistanceType.Poison, 100); // Immune to poison
            SetResistance(ResistanceType.Energy, 50, 65);

            SetSkill(SkillName.EvalInt, 120.0, 140.0);
            SetSkill(SkillName.Magery, 120.0, 150.0);
            SetSkill(SkillName.MagicResist, 120.0, 140.0);
            SetSkill(SkillName.Meditation, 100.0, 120.0);
            SetSkill(SkillName.SpiritSpeak, 120.0, 140.0);
            SetSkill(SkillName.Wrestling, 60.0, 85.0);

            Fame = 25000;
            Karma = -25000;

            VirtualArmor = 60;

            AddItem(new Robe { Hue = 2755, LootType = LootType.Blessed });
            AddItem(new WizardsHat { Hue = 2755, LootType = LootType.Blessed });

            m_NextPhaseShift = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(30, 60));
            m_NextSoulTorrent = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 45));
            m_NextWailOfTheVeil = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(60, 90));
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant == null || !Alive)
                return;

            if (DateTime.UtcNow >= m_NextPhaseShift)
                PhaseShift();

            if (DateTime.UtcNow >= m_NextSoulTorrent)
                SoulTorrent();

            if (DateTime.UtcNow >= m_NextWailOfTheVeil)
                WailOfTheVeil();
        }

        // TELEPORT + VANISH
        private void PhaseShift()
        {
            Effects.SendLocationParticles(EffectItem.Create(Location, Map, TimeSpan.FromSeconds(0.5)), 0x3728, 8, 20, 5042, 0, 5022, 0);
            PlaySound(0x1FE);

            // Teleport to random location within 10 tiles
            Point3D newLoc = Location;
            for (int i = 0; i < 10; i++)
            {
                int x = X + Utility.RandomMinMax(-10, 10);
                int y = Y + Utility.RandomMinMax(-10, 10);
                int z = Map.GetAverageZ(x, y);
                Point3D check = new Point3D(x, y, z);

                if (Map.CanFit(x, y, z, 16, false, false))
                {
                    newLoc = check;
                    break;
                }
            }

            MoveToWorld(newLoc, Map);
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "*vanishes into mist and reappears*");

            m_NextPhaseShift = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(30, 60));
        }

        // DRAIN LIFE FROM ALL NEARBY PLAYERS
        private void SoulTorrent()
        {
            PublicOverheadMessage(MessageType.Regular, 0x22, true, "*channels the souls of the fallen*");
            PlaySound(0x1F9);
            Effects.PlaySound(Location, Map, 0x208);

            foreach (Mobile m in GetMobilesInRange(6))
            {
                if (m != this && m.Alive && m is PlayerMobile)
                {
                    int damage = Utility.RandomMinMax(20, 45);
                    m.FixedParticles(0x374A, 10, 30, 5013, EffectLayer.Head);
                    AOS.Damage(m, this, damage, 0, 0, 100, 0, 0);
                    Hits += damage / 2;
                }
            }

            m_NextSoulTorrent = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 40));
        }

        // HORROR AOE + TEMPORARY BLINDNESS
        private void WailOfTheVeil()
        {
            PublicOverheadMessage(MessageType.Regular, 0x21, true, "*unleashes a psychic wail from beyond the veil*");
            PlaySound(0x5A9);
            Effects.SendLocationEffect(Location, Map, 0x3709, 30, 10);

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m is PlayerMobile pm && pm.Alive)
                {
                    m.FixedParticles(0x374A, 10, 25, 5023, EffectLayer.Waist);
                    m.SendMessage(0x22, "Your vision fades and your mind reels!");
                    m.Paralyze(TimeSpan.FromSeconds(3));
                    m.RevealingAction();
                }
            }

            m_NextWailOfTheVeil = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(60, 90));
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.SuperBoss, 2);
            AddLoot(LootPack.HighScrolls, 3);
            AddLoot(LootPack.Gems, 5);

            if (Utility.RandomDouble() < 0.02) // Rare drop
                PackItem(new ArchmageMantle());
        }

        public override bool AlwaysMurderer => true;
        public override bool CanRummageCorpses => true;
        public override int GetDeathSound() => 0x423;
        public override int GetHurtSound() => 0x436;
        public override int TreasureMapLevel => 5;

        public SepulchralArchmage(Serial serial) : base(serial) { }

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
}
