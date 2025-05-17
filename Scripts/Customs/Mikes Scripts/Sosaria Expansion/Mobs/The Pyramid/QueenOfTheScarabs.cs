using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("a scarab queen corpse")]
    public class QueenOfTheScarabs : BaseCreature
    {
        // Ability cooldowns
        private DateTime m_NextSwarmTime;
        private DateTime m_NextPoisonPoolTime;
        private DateTime m_NextSandTrapTime;
        private DateTime m_NextParalyzeTime;
        private bool m_CarapaceActive;

        // Unique green‑gold hue for the scarab queen
        private const int UniqueHue = 1204;

        [Constructable]
        public QueenOfTheScarabs()
            : base(AIType.AI_Melee, FightMode.Closest, 12, 1, 0.2, 0.4)
        {
            Name = "the Queen of the Scarabs";
            Body = 783;
            BaseSoundID = 959;
            Hue = UniqueHue;

            // Stats
            SetStr(300, 350);
            SetDex(150, 200);
            SetInt(100, 150);

            SetHits(800, 950);
            SetStam(200, 250);
            SetMana(100, 150);

            SetDamage(20, 30);

            // Damage types
            SetDamageType(ResistanceType.Physical, 60);
            SetDamageType(ResistanceType.Poison, 40);

            // Resistances
            SetResistance(ResistanceType.Physical, 30, 40);
            SetResistance(ResistanceType.Fire,     20, 30);
            SetResistance(ResistanceType.Cold,     30, 40);
            SetResistance(ResistanceType.Poison,   50, 60);
            SetResistance(ResistanceType.Energy,   30, 40);

            // Skills
            SetSkill(SkillName.Poisoning,    100.0, 110.0);
            SetSkill(SkillName.MagicResist,   90.0, 100.0);
            SetSkill(SkillName.Tactics,       90.0, 100.0);
            SetSkill(SkillName.Wrestling,     90.0, 100.0);

            Fame = 18000;
            Karma = -18000;
            VirtualArmor = 60;
            ControlSlots = 5;

            // Initialize cooldowns
            var now = DateTime.UtcNow;
            m_NextSwarmTime      = now + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            m_NextPoisonPoolTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
            m_NextSandTrapTime   = now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));
            m_NextParalyzeTime   = now + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));

            // Loot
            PackItem(new Gold(Utility.RandomMinMax(2000, 4000)));
            PackItem(new PoisonPotion());
            if (Utility.RandomDouble() < 0.05)
                PackItem(new Threadbinder()); // unique trophy item
        }

        public QueenOfTheScarabs(Serial serial)
            : base(serial)
        {
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich);
            AddLoot(LootPack.Gems, 5);
            AddLoot(LootPack.MedScrolls, 2);
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant == null || Map == null || !Alive)
                return;

            var now = DateTime.UtcNow;

            // Scarab Swarm: summon small scarab minions around the queen
            if (now >= m_NextSwarmTime)
            {
                ScarabSwarm();
                m_NextSwarmTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            }

            // Poison Pool: drop poison hazard tiles around combatant
            if (now >= m_NextPoisonPoolTime && Combatant is Mobile target1 && InRange(target1.Location, 12))
            {
                PoisonPool(target1);
                m_NextPoisonPoolTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 22));
            }

            // Sand Trap: entombs the current target in quicksand
            if (now >= m_NextSandTrapTime && Combatant is Mobile target2 && InRange(target2, 10))
            {
                SandTrap(target2);
                m_NextSandTrapTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35));
            }

            // Paralyzing Pierce: ranged attack that may paralyze the target
            if (now >= m_NextParalyzeTime && Combatant is Mobile target3 && InRange(target3, 8))
            {
                ParalyzingPierce(target3);
                m_NextParalyzeTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
            }
        }

        // Summon 3–5 scarab minions
        private void ScarabSwarm()
        {
            Say("*The Queen calls forth her brood!*");
            PlaySound(0x582);
            for (int i = 0; i < Utility.RandomMinMax(3, 5); i++)
            {
                var spawn = new Skeleton();  // assume this class exists
                Point3D loc = new Point3D(
                    X + Utility.RandomMinMax(-2, 2),
                    Y + Utility.RandomMinMax(-2, 2),
                    Z
                );
                spawn.MoveToWorld(loc, Map);
            }
        }

        // Creates small poison pools around the target
        private void PoisonPool(Mobile target)
        {
            Say("*Feel the venom of the desert!*");
            PlaySound(0x22F);
            for (int i = 0; i < 3; i++)
            {
                Point3D loc = new Point3D(
                    target.X + Utility.RandomMinMax(-1, 1),
                    target.Y + Utility.RandomMinMax(-1, 1),
                    target.Z
                );
                if (Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                {
                    var tile = new PoisonTile(); 
                    tile.Hue = UniqueHue;
                    tile.MoveToWorld(loc, Map);
                }
            }
        }

        // Traps the target in quicksand, slowing and damaging over time
        private void SandTrap(Mobile target)
        {
            Say("*The sands shall bind you!*");
            PlaySound(0x214);
            Point3D loc = target.Location;
            if (Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
            {
                var sand = new QuicksandTile();
                sand.Hue = UniqueHue;
                sand.MoveToWorld(loc, Map);
            }
        }

        // Fires a barbed scarab projectile that can paralyze
        private void ParalyzingPierce(Mobile target)
        {
            Say("*Behold my barbs!*");
            PlaySound(0x195);
            Effects.SendMovingParticles(
                new Entity(Serial.Zero, Location, Map),
                new Entity(Serial.Zero, target.Location, target.Map),
                0x36D4, 7, 0, false, false, 0x3E, 0, 9502, 1, 0, EffectLayer.Waist, 0
            );

            Timer.DelayCall(TimeSpan.FromSeconds(0.4), () =>
            {
                if (target.Alive && CanBeHarmful(target, false))
                {
                    DoHarmful(target);
                    AOS.Damage(target, this, Utility.RandomMinMax(30, 45), 100, 0, 0, 0, 0);

                    if (Utility.RandomDouble() < 0.3)
                    {
                        target.Paralyze(TimeSpan.FromSeconds(4.0));
                        target.SendMessage(0x22, "You feel the venomous paralysis creep through your veins!");
                    }
                }
            });
        }

        public override void OnDamage(int amount, Mobile from, bool willKill)
        {
            base.OnDamage(amount, from, willKill);

            // Activate Carapace Shield once when below 50% health
            if (!m_CarapaceActive && !willKill && Hits < (HitsMax / 2))
            {
                m_CarapaceActive = true;
                ActivateCarapaceShield();
            }
        }

        // Temporarily boosts resistances and deals poison burst
        private void ActivateCarapaceShield()
        {
            Say("*My carapace hardens!*");
            PlaySound(0x2F4);
            FixedParticles(0x376A, 10, 20, 5032, UniqueHue, 0, EffectLayer.Waist);

            // Boost resistances by +20
            this.SetResistance(ResistanceType.Physical, this.PhysicalResistance + 20);
            this.SetResistance(ResistanceType.Poison,   this.PoisonResistance   + 20);

            // Schedule shield drop and poison burst
            Timer.DelayCall(TimeSpan.FromSeconds(10.0), () =>
            {
                if (!Deleted)
                {
                    Say("*The carapace crumbles!*");
                    PlaySound(0x214);
                    FixedParticles(0x3779, 10, 25, 5032, UniqueHue, 0, EffectLayer.Head);

                    // Revert resistances
                    this.SetResistance(ResistanceType.Physical, this.PhysicalResistance - 20);
                    this.SetResistance(ResistanceType.Poison,   this.PoisonResistance   - 20);
                }
            });
        }

        public override bool BleedImmune { get { return true; } }
        public override Poison PoisonImmune { get { return Poison.Lethal; } }
        public override int TreasureMapLevel { get { return 6; } }
        public override double DispelDifficulty { get { return 125.0; } }
        public override double DispelFocus     { get { return 60.0;  } }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(m_CarapaceActive);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            if (version >= 0)
                m_CarapaceActive = reader.ReadBool();

            // reset cooldowns on reload
            var now = DateTime.UtcNow;
            m_NextSwarmTime      = now + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            m_NextPoisonPoolTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
            m_NextSandTrapTime   = now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));
            m_NextParalyzeTime   = now + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
        }
    }
}
