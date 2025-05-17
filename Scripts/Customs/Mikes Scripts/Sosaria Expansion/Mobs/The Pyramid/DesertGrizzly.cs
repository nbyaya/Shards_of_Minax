using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a desert grizzly corpse")]
    public class DesertGrizzly : BaseCreature
    {
        private const int DesertHue = 2206; // Sandy‐gold tone

        // Cooldowns for special abilities
        private DateTime m_NextSandRoar;
        private DateTime m_NextQuicksand;
        private DateTime m_NextScarabSwarm;

        [Constructable]
        public DesertGrizzly() 
            : base(AIType.AI_Melee, FightMode.Closest, 12, 1, 0.2, 0.4)
        {
            Name = "a Desert Grizzly";
            Body = 212;
            BaseSoundID = 0xA3;
            Hue = DesertHue;

            // --- Stats ---
            SetStr(500, 550);
            SetDex(150, 200);
            SetInt(75, 100);

            SetHits(200, 250);
            SetStam(300, 350);
            SetMana(100); // barely any

            SetDamage(25, 35);

            // 50% Physical, 50% Fire
            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 50);

            // Resistances
            SetResistance(ResistanceType.Physical, 60, 70);
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 20, 30);
            SetResistance(ResistanceType.Poison, 40, 50);
            SetResistance(ResistanceType.Energy, 30, 40);

            // Skills
            SetSkill(SkillName.Tactics, 100.1, 120.0);
            SetSkill(SkillName.Wrestling, 100.1, 120.0);
            SetSkill(SkillName.MagicResist, 90.1, 110.0);

            Fame = 25000;
            Karma = -25000;
            VirtualArmor = 90;
            ControlSlots = 6;

            // Initialize cooldowns
            var now = DateTime.UtcNow;
            m_NextSandRoar   = now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            m_NextQuicksand  = now + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 25));
            m_NextScarabSwarm= now + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35));

            // Loot
            PackItem(new IronOre(Utility.RandomMinMax(20, 40)));
            PackGold(2000, 4000);
        }

        // --- Aura: scorching sands on approach ---
        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            base.OnMovement(m, oldLocation);

            if (m != null && m != this && m.Map == Map && Alive && m.InRange(Location, 2) && CanBeHarmful(m, false))
            {
                if (m is Mobile target)
                {
                    DoHarmful(target);
                    int dmg = Utility.RandomMinMax(10, 20);
                    AOS.Damage(target, this, dmg, 0, 100, 0, 0, 0); // pure fire
                    target.SendMessage(0x22, "The sands around the beast scorch you!"); 
                    target.PlaySound(0x208);
                }
            }
        }

        // --- Think: trigger special attacks if off‐cooldown ---
        public override void OnThink()
        {
            base.OnThink();

            if (Combatant == null || !Alive || Map == null || Map == Map.Internal)
                return;

            var now = DateTime.UtcNow;

            if (now >= m_NextSandRoar && InRange(Combatant.Location, 10))
            {
                SandstormRoar();
                m_NextSandRoar = now + TimeSpan.FromSeconds(Utility.RandomMinMax(30, 40));
            }
            else if (now >= m_NextQuicksand && InRange(Combatant.Location, 12))
            {
                QuicksandTrap();
                m_NextQuicksand = now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            }
            else if (now >= m_NextScarabSwarm && InRange(Combatant.Location, 8))
            {
                SummonScarabSwarm();
                m_NextScarabSwarm = now + TimeSpan.FromSeconds(Utility.RandomMinMax(35, 50));
            }
        }

        // --- Ability 1: Sandstorm Roar (AoE fire + brief blindness) ---
        public void SandstormRoar()
        {
            Say("*a deafening roar fills the cavern!*");
            PlaySound(0x2F3); // roar

            Effects.SendLocationParticles(
                EffectItem.Create(Location, Map, EffectItem.DefaultDuration),
                0x374A, 20, 50, DesertHue, 0, 5023, 0);

            var targets = new List<Mobile>();
            foreach (Mobile m in Map.GetMobilesInRange(Location, 8))
            {
                if (m != this && m.Alive && CanBeHarmful(m, false) && m is Mobile tgt)
                    targets.Add(tgt);
            }

            foreach (Mobile tgt in targets)
            {
                DoHarmful(tgt);
                int dmg = Utility.RandomMinMax(40, 60);
                AOS.Damage(tgt, this, dmg, 0, 100, 0, 0, 0);

                // Simulate brief blindness via paralyze
                tgt.SendMessage(0x22, "The sand stings your eyes—you reel!");
                tgt.FixedParticles(0x3779, 10, 15, 5032, DesertHue, 0, EffectLayer.Head);
                tgt.Paralyze(TimeSpan.FromSeconds(2));
            }
        }

        // --- Ability 2: Quicksand Trap (places 1 QuicksandTile under the target) ---
        public void QuicksandTrap()
        {
            if (Combatant is Mobile target && CanBeHarmful(target, false))
            {
                Say("*the earth shifts beneath you!*");
                PlaySound(0x11D);

                var loc = target.Location;
                Timer.DelayCall(TimeSpan.FromSeconds(0.5), () =>
                {
                    if (Map == null) return;

                    // adjust Z if needed
                    var z = Map.GetAverageZ(loc.X, loc.Y);
                    if (!Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                        loc.Z = z;

                    var tile = new QuicksandTile {
                        Hue = DesertHue
                    };
                    tile.MoveToWorld(loc, Map);
                });
            }
        }

        // --- Ability 3: Summon Scarab Swarm (spawns 3 tiny desert scarabs) ---
        public void SummonScarabSwarm()
        {
            Say("*chittering echoes across the sands!*");
            PlaySound(0x2E3);

            for (int i = 0; i < 3; i++)
            {
                var spawn = new Scorpion();      // reusing scorpion as “scarab”
                spawn.Name = "a desert scarab";
                spawn.Hue  = DesertHue;
                spawn.Team = this.Team;             // fights your enemies
                spawn.MoveToWorld(Location, Map);
            }
        }

        // --- OnDeath: fiery explosion + quicksand puddles ---
        public override void OnDeath(Container c)
        {
            base.OnDeath(c);
			
			if (Map == null) return;
			Say("*the sands erupt in flame!*");
            Effects.PlaySound(Location, Map, 0x208);
            Effects.SendLocationParticles(
                EffectItem.Create(Location, Map, EffectItem.DefaultDuration),
                0x3709, 20, 60, DesertHue, 0, 5052, 0);

            for (int i = 0; i < 5; i++)
            {
                int x = X + Utility.RandomMinMax(-3, 3);
                int y = Y + Utility.RandomMinMax(-3, 3);
                int z = Z;
                if (!Map.CanFit(x, y, z, 16, false, false))
                    z = Map.GetAverageZ(x, y);

                var puddle = new QuicksandTile {
                    Hue = DesertHue
                };
                puddle.MoveToWorld(new Point3D(x, y, z), Map);
            }

            
        }

        // --- Loot & properties ---
        public override bool BleedImmune    { get { return true; } }
        public override Poison PoisonImmune { get { return Poison.Lethal; } }
        public override int TreasureMapLevel{ get { return 5; } }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich);
            PackItem(new Granite(Utility.RandomMinMax(10, 20)));
            if (Utility.RandomDouble() < 0.05)
                PackItem(new HearthstitchWrap()); // 5% unique drop
        }

        public DesertGrizzly(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            
            // reset cooldowns on load
            var now = DateTime.UtcNow;
            m_NextSandRoar    = now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            m_NextQuicksand   = now + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 25));
            m_NextScarabSwarm = now + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35));
        }
    }
}
