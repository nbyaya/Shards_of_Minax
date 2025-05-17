using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Items;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("a sunken aurum wraith corpse")]
    public class SunkenAurumWraith : BaseCreature
    {
        private DateTime m_NextGoldDrain;
        private DateTime m_NextHauntingWail;
        private DateTime m_NextCoinTorrent;
        private bool m_AbilitiesInitialized;

        [Constructable]
        public SunkenAurumWraith()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a Sunken Aurum Wraith";
            Body = 166;
            BaseSoundID = 268;
            Hue = 2213; // Pale golden spectral shimmer

            SetStr(325, 375);
            SetDex(151, 200);
            SetInt(250, 300);

            SetHits(750, 950);
            SetMana(1000);

            SetDamage(20, 30);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Energy, 50);

            SetResistance(ResistanceType.Physical, 60, 75);
            SetResistance(ResistanceType.Fire, 30, 40);
            SetResistance(ResistanceType.Cold, 60, 75);
            SetResistance(ResistanceType.Poison, 70, 90);
            SetResistance(ResistanceType.Energy, 65, 80);

            SetSkill(SkillName.EvalInt, 100.0, 120.0);
            SetSkill(SkillName.Magery, 110.0, 130.0);
            SetSkill(SkillName.MagicResist, 100.0, 120.0);
            SetSkill(SkillName.Tactics, 90.0, 110.0);
            SetSkill(SkillName.Wrestling, 90.0, 100.0);

            Fame = 24000;
            Karma = -24000;

            VirtualArmor = 80;

            m_AbilitiesInitialized = false;
        }

        public SunkenAurumWraith(Serial serial)
            : base(serial)
        {
        }

        public override bool AutoDispel => true;
        public override bool BleedImmune => true;
        public override bool Unprovokable => true;
        public override int TreasureMapLevel => 4;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich, 2);
            AddLoot(LootPack.Gems, 6);
            if (Utility.RandomDouble() < 0.005) // 1 in 200 chance
            {
                PackItem(new CursedGoldMask()); // Custom item
            }
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    m_NextGoldDrain = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 20));
                    m_NextHauntingWail = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 40));
                    m_NextCoinTorrent = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 30));
                    m_AbilitiesInitialized = true;
                }

                if (DateTime.UtcNow >= m_NextGoldDrain)
                    GoldDrain();

                if (DateTime.UtcNow >= m_NextHauntingWail)
                    HauntingWail();

                if (DateTime.UtcNow >= m_NextCoinTorrent)
                    CoinTorrent();
            }
        }

        private void GoldDrain()
        {
            PublicOverheadMessage(MessageType.Emote, 0x22, false, "*The Sunken Wraith whispers a curse of greed...*");
            PlaySound(0x1F7); // Ethereal sound

            if (Combatant is Mobile target && target.Backpack != null)
            {
                var gold = target.Backpack.FindItemByType(typeof(Gold));
                if (gold is Gold playerGold && playerGold.Amount > 0)
                {
                    int amount = Math.Min(Utility.RandomMinMax(20, 100), playerGold.Amount);
                    playerGold.Amount -= amount;
                    PackItem(new Gold(amount));

                    target.SendMessage(0x22, "Your gold vanishes into spectral mist!");
                }
            }

            m_NextGoldDrain = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(30, 60));
        }

        private void HauntingWail()
        {
            PublicOverheadMessage(MessageType.Emote, 0x22, false, "*A soul-rending wail echoes from beneath the waves...*");
            PlaySound(0x5C3); // Wailing

            Effects.SendLocationEffect(Location, Map, 0x3709, 15, 10);

            foreach (Mobile m in GetMobilesInRange(6))
            {
                if (m != this && m.Alive && m is PlayerMobile)
                {
                    m.FixedParticles(0x37B9, 10, 30, 5012, EffectLayer.Head);
                    m.SendMessage(0x22, "Your mind reels from the wraith's unholy scream!");
                    m.Stam -= Utility.RandomMinMax(10, 25);
                    m.Mana -= Utility.RandomMinMax(10, 20);
                }
            }

            m_NextHauntingWail = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(45, 90));
        }

        private void CoinTorrent()
        {
            PublicOverheadMessage(MessageType.Emote, 0x22, false, "*A torrent of ghostly coins bursts forth!*");
            PlaySound(0x2CE); // Coin clatter

            Effects.SendLocationEffect(Location, Map, 0x36BD, 20, 10);

            foreach (Mobile m in GetMobilesInRange(4))
            {
                if (m != this && m.Alive)
                {
                    AOS.Damage(m, this, Utility.RandomMinMax(15, 35), 0, 100, 0, 0, 0);
                    if (m is Mobile mob)
                    {
                        mob.SendMessage("The spectral gold sears your flesh with greedy fire!");
                        mob.ApplyPoison(this, Poison.Regular);
                    }
                }
            }

            m_NextCoinTorrent = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(30, 50));
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
            m_AbilitiesInitialized = false;
        }
    }

    public class CursedGoldMask : Item
    {
        [Constructable]
        public CursedGoldMask() : base(0x2B70) // Custom item graphic
        {
            Name = "Cursed Gold Mask";
            Hue = 2213;
            LootType = LootType.Cursed;
            Weight = 1.0;
        }

        public CursedGoldMask(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
