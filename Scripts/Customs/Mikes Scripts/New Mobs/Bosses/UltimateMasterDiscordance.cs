using System;
using System.Collections;
using Server.Engines.CannedEvil;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("corpse of Orpheus")]
    public class UltimateMasterDiscordance : BaseChampion
    {
        private DateTime m_NextAbilityTime;

        [Constructable]
        public UltimateMasterDiscordance()
            : base(AIType.AI_Mage)
        {
            Name = "Orpheus";
            Title = "The Master of Discordance";
            Body = 0x190;
            Hue = 0x83F;

            SetStr(300, 420);
            SetDex(80, 150);
            SetInt(510, 760);

            SetHits(11500);
            SetMana(2600);

            SetDamage(20, 30);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 55, 65);
            SetResistance(ResistanceType.Fire, 50, 60);
            SetResistance(ResistanceType.Cold, 50, 60);
            SetResistance(ResistanceType.Poison, 50, 60);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.Musicianship, 120.0);
            SetSkill(SkillName.Discordance, 120.0);
            SetSkill(SkillName.Provocation, 120.0);
            SetSkill(SkillName.Peacemaking, 120.0);
            SetSkill(SkillName.MagicResist, 120.0);
            SetSkill(SkillName.Tactics, 100.0);
            SetSkill(SkillName.Wrestling, 100.0);

            Fame = 23000;
            Karma = -23000;

            VirtualArmor = 60;

            AddItem(new FancyShirt(Utility.RandomRedHue()));
            AddItem(new LongPants(Utility.RandomBlueHue()));
            AddItem(new Sandals());

            HairItemID = 0x204B; // Long Hair
            HairHue = 0x47E;
        }

        public UltimateMasterDiscordance(Serial serial)
            : base(serial)
        {
        }

        public override ChampionSkullType SkullType { get { return ChampionSkullType.Power; } }

        public override Type[] UniqueList
        {
            get { return new Type[] { typeof(LyreOfDiscord), typeof(HarmoniousCloak) }; }
        }

        public override Type[] SharedList
        {
            get { return new Type[] { typeof(LyreOfDiscord), typeof(HarmoniousCloak) }; }
        }

        public override Type[] DecorativeList
        {
            get { return new Type[] { typeof(LyreOfDiscord), typeof(HarmoniousCloak) }; }
        }

        public override MonsterStatuetteType[] StatueTypes
        {
            get { return new MonsterStatuetteType[] { }; }
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 3);
            AddLoot(LootPack.FilthyRich);
            AddLoot(LootPack.Gems, 6);
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            c.DropItem(new PowerScroll(SkillName.Discordance, 200.0));

            if (Utility.RandomDouble() < 0.6)
                c.DropItem(new LyreOfDiscord());

            if (Utility.RandomDouble() < 0.6)
                c.DropItem(new HarmoniousCloak());
        }

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            if (DateTime.Now > m_NextAbilityTime)
            {
                switch (Utility.Random(3))
                {
                    case 0: MelodicChaos(); break;
                    case 1: Harmony(); break;
                    case 2: DiscordantWave(); break;
                }

                m_NextAbilityTime = DateTime.Now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            }
        }

        public void MelodicChaos()
        {
            ArrayList targets = new ArrayList();

            foreach (Mobile m in this.GetMobilesInRange(8))
            {
                if (m != this && m.Player && this.CanBeHarmful(m))
                    targets.Add(m);
            }

            for (int i = 0; i < targets.Count; ++i)
            {
                Mobile m = (Mobile)targets[i];

                DoHarmful(m);

                m.Paralyzed = true;
                m.FixedParticles(0x373A, 10, 15, 5036, EffectLayer.Head);
                m.PlaySound(0x1F9);
            }
        }

        public void Harmony()
        {
            ArrayList targets = new ArrayList();

            foreach (Mobile m in this.GetMobilesInRange(8))
            {
                if (m.Player && this.CanBeBeneficial(m))
                    targets.Add(m);
            }

            for (int i = 0; i < targets.Count; ++i)
            {
                Mobile m = (Mobile)targets[i];

                DoBeneficial(m);

                m.FixedParticles(0x376A, 9, 32, 5008, EffectLayer.Waist);
                m.PlaySound(0x1E9);

                m.Hits += Utility.RandomMinMax(20, 40);
                m.Stam += Utility.RandomMinMax(20, 40);
                m.Mana += Utility.RandomMinMax(20, 40);
            }
        }

        public void DiscordantWave()
        {
            ArrayList targets = new ArrayList();

            foreach (Mobile m in this.GetMobilesInRange(8))
            {
                if (m != this && m.Player && this.CanBeHarmful(m))
                    targets.Add(m);
            }

            for (int i = 0; i < targets.Count; ++i)
            {
                Mobile m = (Mobile)targets[i];

                DoHarmful(m);

                AOS.Damage(m, this, Utility.RandomMinMax(40, 60), 0, 100, 0, 0, 0);

                m.FixedParticles(0x36BD, 20, 10, 5044, EffectLayer.Head);
                m.PlaySound(0x210);
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

    public class LyreOfDiscord : Item
    {
        [Constructable]
        public LyreOfDiscord() : base(0xEB2)
        {
            Name = "Lyre of Discord";
            Hue = 0x47E;
        }

        public LyreOfDiscord(Serial serial) : base(serial)
        {
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

    public class HarmoniousCloak : Item
    {
        [Constructable]
        public HarmoniousCloak() : base(0x1515)
        {
            Name = "Harmonious Cloak";
            Hue = 0x47E;
        }

        public HarmoniousCloak(Serial serial) : base(serial)
        {
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
}
