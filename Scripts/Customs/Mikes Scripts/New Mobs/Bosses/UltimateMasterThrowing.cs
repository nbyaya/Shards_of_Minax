using System;
using System.Collections;
using Server.Engines.CannedEvil;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("corpse of William Tell")]
    public class UltimateMasterThrowing : BaseChampion
    {
        private DateTime m_NextAbilityTime;

        [Constructable]
        public UltimateMasterThrowing()
            : base(AIType.AI_Archer)
        {
            Name = "William Tell";
            Title = "The Legendary Marksman";
            Body = 0x190;
            Hue = 0x83F;

            SetStr(350, 450);
            SetDex(250, 350);
            SetInt(150, 200);

            SetHits(15000);
            SetMana(2000);

            SetDamage(30, 40);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 20);
            SetDamageType(ResistanceType.Cold, 20);
            SetDamageType(ResistanceType.Poison, 10);

            SetResistance(ResistanceType.Physical, 70, 80);
            SetResistance(ResistanceType.Fire, 50, 60);
            SetResistance(ResistanceType.Cold, 50, 60);
            SetResistance(ResistanceType.Poison, 70, 80);
            SetResistance(ResistanceType.Energy, 40, 50);

            SetSkill(SkillName.Archery, 120.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.MagicResist, 100.0);
            SetSkill(SkillName.Anatomy, 120.0);

            Fame = 24000;
            Karma = -24000;

            VirtualArmor = 75;
            
            AddItem(new ChainChest());
            AddItem(new ChainLegs());
            AddItem(new Boots());
            AddItem(new Cloak(Utility.RandomRedHue()));
            AddItem(new Bow());

            HairItemID = 0x203B; // Short Hair
            HairHue = 0x44E;
        }

        public UltimateMasterThrowing(Serial serial)
            : base(serial)
        {
        }

        public override ChampionSkullType SkullType { get { return ChampionSkullType.Power; } }

        public override Type[] UniqueList
        {
            get { return new Type[] { typeof(PrecisionBolts), typeof(TellGlove) }; }
        }

        public override Type[] SharedList
        {
            get { return new Type[] { typeof(ArcherTalisman), typeof(Quiver) }; }
        }

        public override Type[] DecorativeList
        {
            get { return new Type[] { typeof(TrophyArrow), typeof(BowStand) }; }
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

            c.DropItem(new PowerScroll(SkillName.Throwing, 200.0));

            if (Utility.RandomDouble() < 0.6)
                c.DropItem(new PrecisionBolts());

            if (Utility.RandomDouble() < 0.6)
                c.DropItem(new TellGlove());
        }

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            if (DateTime.Now > m_NextAbilityTime)
            {
                switch (Utility.Random(3))
                {
                    case 0: PiercingThrow(defender); break;
                    case 1: RapidFire(); break;
                    case 2: Ricochet(defender); break;
                }

                m_NextAbilityTime = DateTime.Now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            }
        }

        public void PiercingThrow(Mobile defender)
        {
            if (defender != null)
            {
                DoHarmful(defender);
                defender.Damage(Utility.RandomMinMax(80, 100), this);
                defender.FixedParticles(0x36BD, 20, 10, 5044, EffectLayer.Head);
                defender.PlaySound(0x207);
            }
        }

        public void RapidFire()
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
                m.Damage(Utility.RandomMinMax(30, 50), this);

                m.FixedParticles(0x36BD, 20, 10, 5044, EffectLayer.Head);
                m.PlaySound(0x207);
            }
        }

        public void Ricochet(Mobile defender)
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
                m.Damage(Utility.RandomMinMax(30, 50), this);

                m.FixedParticles(0x36BD, 20, 10, 5044, EffectLayer.Head);
                m.PlaySound(0x207);
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
	
public class PrecisionBolts : Item
{
    [Constructable]
    public PrecisionBolts() : base(0x1BFB)
    {
        Name = "Precision Bolts";
        Hue = 0x44E;
    }

    public PrecisionBolts(Serial serial) : base(serial)
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

public class TellGlove : Item
{
    [Constructable]
    public TellGlove() : base(0x13C6)
    {
        Name = "Tell's Glove";
        Hue = 0x44E;
    }

    public TellGlove(Serial serial) : base(serial)
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


