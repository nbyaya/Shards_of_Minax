using System;
using System.Collections;
using Server;
using Server.Items;
using Server.Misc;
using Server.Mobiles;
using Server.Spells;
using Server.Spells.Necromancy;
using Server.Spells.Spellweaving;

namespace Server.Mobiles
{
    [CorpseName("a collapsing singularity")]
    public class VoidbornAbomination : BaseCreature
    {
        private DateTime m_NextPhaseShift;
        private int m_PhaseCount;
        private bool m_IsPhasing;

        [Constructable]
        public VoidbornAbomination() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "The Voidborn Abomination";
            Body = 311;
            Hue = 2075;
            BaseSoundID = 357;

            SetStr(650, 800);
            SetDex(150, 200);
            SetInt(500, 600);

            SetHits(5000, 6500);
            SetStam(300, 400);
            SetMana(1000, 1200);

            SetDamage(25, 35);

            SetDamageType(ResistanceType.Physical, 20);
            SetDamageType(ResistanceType.Fire, 20);
            SetDamageType(ResistanceType.Cold, 20);
            SetDamageType(ResistanceType.Poison, 20);
            SetDamageType(ResistanceType.Energy, 20);

            SetResistance(ResistanceType.Physical, 75, 85);
            SetResistance(ResistanceType.Fire, 70, 80);
            SetResistance(ResistanceType.Cold, 70, 80);
            SetResistance(ResistanceType.Poison, 80, 90);
            SetResistance(ResistanceType.Energy, 80, 90);

            SetSkill(SkillName.Magery, 120.0);
            SetSkill(SkillName.EvalInt, 120.0);
            SetSkill(SkillName.Meditation, 120.0);
            SetSkill(SkillName.MagicResist, 150.0);
            SetSkill(SkillName.Tactics, 100.0);
            SetSkill(SkillName.Wrestling, 100.0);
            SetSkill(SkillName.Anatomy, 50.0);

            Fame = 25000;
            Karma = -25000;

            VirtualArmor = 80;

            // Unique Loot
            PackItem(new BlackEssence());
            
            m_NextPhaseShift = DateTime.Now;
            m_PhaseCount = 0;
            m_IsPhasing = false;
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.SuperBoss, 3);
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            // 10% chance to drop special artifact
            if (Utility.RandomDouble() < 0.10)
                c.DropItem(new VoidbornScepter());
                
            // Create collapsing singularity effect
            Timer.DelayCall(TimeSpan.FromSeconds(5), () =>
            {
                Effects.SendLocationEffect(c.Location, c.Map, 0x36BD, 20, 10, 2075, 0);
                Effects.PlaySound(c.Location, c.Map, 0x307);
                
                foreach (Mobile m in GetMobilesInRange(5))
                {
                    if (m != null && m != this && m.Alive)
                        m.Damage(Utility.RandomMinMax(50, 100), this);
                }
            });
        }

        public override void OnDamage(int amount, Mobile from, bool willKill)
        {
            if (Hits < HitsMax * 0.7 && m_PhaseCount < 3 && !m_IsPhasing)
            {
                PhaseShift();
            }

            base.OnDamage(amount, from, willKill);
        }

        private void PhaseShift()
        {
            m_IsPhasing = true;
            m_PhaseCount++;
            
            // Become temporarily invulnerable
            Blessed = true;
            FixedEffect(0x376A, 10, 16);
            PlaySound(0x202);
            PublicOverheadMessage(Network.MessageType.Regular, 0x3B2, true, "The abomination phases between realities!");

            Timer.DelayCall(TimeSpan.FromSeconds(10), () =>
            {
                Blessed = false;
                m_IsPhasing = false;
                Combatant = ControlMaster; // Re-engage combat
            });

            // Spawn void fragments during phase shift
            for (int i = 0; i < 3; i++)
            {
                SpawnVoidFragment();
            }
        }

        private void SpawnVoidFragment()
        {
            VoidFragment fragment = new VoidFragment();
            fragment.MoveToWorld(GetSpawnPosition(3), Map);
            fragment.Combatant = Combatant;
        }

        public override void OnThink()
        {
            base.OnThink();

            if (DateTime.Now >= m_NextPhaseShift && Combatant != null)
            {
                // Random special ability
                switch (Utility.Random(4))
                {
                    case 0: UnstableBurst(); break;
                    case 1: TemporalDisplacement(); break;
                    case 2: SummonVoidSpawns(); break;
                    case 3: CorruptingTouch(); break;
                }

                m_NextPhaseShift = DateTime.Now + TimeSpan.FromSeconds(Utility.RandomMinMax(7, 10));
            }
        }

        private void UnstableBurst()
        {
            FixedEffect(0x36BD, 20, 10);
            PlaySound(0x307);
            
            foreach (Mobile m in GetMobilesInRange(8))
            {
                if (m != null && m != this && CanBeHarmful(m))
                {
                    m.Damage(Utility.RandomMinMax(30, 50), this);
                    m.SendMessage("You're struck by chaotic energy!");
                }
            }
        }

        private void TemporalDisplacement()
        {
            Say("I UNMAKE YOUR TIME!");
            new InternalTimer(this).Start();
        }

        private class InternalTimer : Timer
        {
            private Mobile m_Owner;

            public InternalTimer(Mobile owner) : base(TimeSpan.FromSeconds(2))
            {
                m_Owner = owner;
            }

            protected override void OnTick()
            {
                if (m_Owner is VoidbornAbomination abom)
                {
                    foreach (Mobile m in abom.GetMobilesInRange(12))
                    {
                        if (m != null && m != abom && m.Player)
                        {
                            m.Freeze(TimeSpan.FromSeconds(5));
                            m.SendMessage("You're frozen in fractured time!");
                        }
                    }
                }
            }
        }

        private void SummonVoidSpawns()
        {
            int count = Utility.RandomMinMax(2, 4);
            for (int i = 0; i < count; i++)
            {
                BaseCreature spawn = new Spectre();
                spawn.MoveToWorld(GetSpawnPosition(5), Map);
                spawn.Combatant = Combatant;
            }
        }

        private void CorruptingTouch()
        {
            if (Combatant is Mobile m)
            {
                m.AddStatMod(new StatMod(StatType.Str, "Void Touch", -20, TimeSpan.FromSeconds(30)));
                m.SendMessage("Your strength drains into the void!");
                m.FixedEffect(0x37CC, 10, 20, 2075, 0);
            }
        }

        public override bool Unprovokable => true;
        public override bool BardImmune => true;
        public override Poison PoisonImmune => Poison.Lethal;
        public override bool AutoDispel => true;
        public override bool CanRummageCorpses => false;
        public override bool AlwaysMurderer => true;

        public VoidbornAbomination(Serial serial) : base(serial)
        {
        }

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

    // Supporting Mobiles and Items
    public class VoidFragment : BaseCreature
    {
        [Constructable]
        public VoidFragment() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a void fragment";
            Body = 51;
            Hue = 2075;
            BaseSoundID = 456;

            SetStr(150, 200);
            SetHits(200, 300);
            SetDamage(15, 25);

            SetResistance(ResistanceType.Physical, 60, 70);
            SetResistance(ResistanceType.Energy, 80, 90);

            VirtualArmor = 40;
        }

        public override void OnDeath(Container c)
        {
            Effects.SendLocationEffect(c.Location, c.Map, 0x374A, 20);
            base.OnDeath(c);
        }

        public VoidFragment(Serial serial) : base(serial)
        {
        }

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

    public class BlackEssence : Item
    {
        [Constructable]
        public BlackEssence() : base(0x1F1B)
        {
            Name = "Void Essence";
            Hue = 2075;
            Stackable = true;
        }

        public BlackEssence(Serial serial) : base(serial)
        {
        }

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

    public class VoidbornScepter : Item
    {
        [Constructable]
        public VoidbornScepter() : base(0x26C0)
        {
            Name = "Scepter of the Voidborn";
            Hue = 2075;
            Weight = 5.0;
            LootType = LootType.Blessed;
        }

        public override void OnDoubleClick(Mobile from)
        {
            from.SendMessage("A weapon of unimaginable power... perhaps a skilled blacksmith can unlock its potential.");
        }

        public VoidbornScepter(Serial serial) : base(serial)
        {
        }

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