using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a phasebeak corpse")]
    public class Phasebeak : BaseCreature
    {
        // Ability cooldowns
        private DateTime m_NextSonicScreech;
        private DateTime m_NextPhaseVolley;
        private DateTime m_NextRiftSpawn;
        private Point3D m_LastLocation;

        // Unique iridescent violet hue
        private const int UniqueHue = 1258;

        [Constructable]
        public Phasebeak() 
            : base(AIType.AI_Melee | AIType.AI_Mage, FightMode.Closest, 12, 1, 0.2, 0.4)
        {
            Name = "a Phasebeak";
            Body = 0xDA;               // As FrenziedOstard
            BaseSoundID = 0x275;       // As FrenziedOstard
            Hue = UniqueHue;

            // —— Core Stats ——  
            SetStr(350, 450);
            SetDex(200, 250);
            SetInt(300, 400);

            SetHits(1200, 1500);
            SetStam(200, 250);
            SetMana(350, 450);

            SetDamage(20, 30);

            // —— Damage Types ——  
            SetDamageType(ResistanceType.Physical, 30);
            SetDamageType(ResistanceType.Cold, 35);
            SetDamageType(ResistanceType.Energy, 35);

            // —— Resistances ——  
            SetResistance(ResistanceType.Physical, 45, 55);
            SetResistance(ResistanceType.Fire, 40, 50);
            SetResistance(ResistanceType.Cold, 60, 70);
            SetResistance(ResistanceType.Poison, 30, 40);
            SetResistance(ResistanceType.Energy, 75, 85);

            // —— Skills ——  
            SetSkill(SkillName.Wrestling, 100.0, 115.0);
            SetSkill(SkillName.Tactics, 100.0, 115.0);
            SetSkill(SkillName.MagicResist, 120.0, 135.0);
            SetSkill(SkillName.EvalInt, 110.0, 125.0);
            SetSkill(SkillName.Magery, 110.0, 125.0);

            Fame = 25000;
            Karma = -25000;

            VirtualArmor = 90;
            ControlSlots = 6;

            // Initialize cooldowns
            var now = DateTime.UtcNow;
            m_NextSonicScreech = now + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 14));
            m_NextPhaseVolley   = now + TimeSpan.FromSeconds(Utility.RandomMinMax(18, 24));
            m_NextRiftSpawn     = now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 28));

            m_LastLocation = this.Location;

            // Starter loot: planar fragments
            PackItem(new PowerCrystal(Utility.RandomMinMax(5, 10)));
            PackItem(new VoidCore(Utility.RandomMinMax(2, 5)));
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant == null || !Alive || Map == null || Map == Map.Internal)
                return;

            var now = DateTime.UtcNow;

            // Sonic Screech: frontal cone stun + bleed
            if (now >= m_NextSonicScreech && this.InRange(Combatant.Location, 6))
            {
                SonicScreech();
                m_NextSonicScreech = now + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
                return;
            }

            // Phase Volley: lobs energy bolts at up to 4 nearby targets
            if (now >= m_NextPhaseVolley && this.InRange(Combatant.Location, 12))
            {
                PhaseVolley();
                m_NextPhaseVolley = now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 26));
                return;
            }

            // Rift Spawn: drop a planar hazard tile at its current location
            if (now >= m_NextRiftSpawn)
            {
                SpawnPlanarRift();
                m_NextRiftSpawn = now + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35));
            }
        }

        // —— Ability 1: Sonic Screech ——  
        private void SonicScreech()
        {
            this.Say("*KREEE-AK!*");
            PlaySound(0x2E8); // harsh bird screech

            // Cone in front of Phasebeak
            var targets = new List<Mobile>();
            IPooledEnumerable inRange = Map.GetMobilesInRange(this.Location, 6);
            foreach (Mobile m in inRange)
            {
                if (m == this || !CanBeHarmful(m, false))
                    continue;

                // Rough cone check: target must be generally in front
                if (this.InLOS(m) && this.GetDirectionTo(m.Location) == this.Direction)
                    targets.Add(m);
            }
            inRange.Free();

            foreach (var m in targets)
            {
                DoHarmful(m);
                int damage = Utility.RandomMinMax(25, 40);
                AOS.Damage(m, this, damage, 30, 20, 0, 0, 50);

                if (m is Mobile target)
                {
                    // Apply bleed
                    target.ApplyPoison(this, Poison.Lethal);
                    target.SendMessage(0x22, "You reel from the devastating screech!");
                }
            }
        }

        // —— Ability 2: Phase Volley ——  
        private void PhaseVolley()
        {
            this.Say("*Shifting…*");
            PlaySound(0x1FE); // magical whoosh

            var potential = new List<Mobile>();
            IPooledEnumerable nearby = Map.GetMobilesInRange(this.Location, 12);
            foreach (Mobile m in nearby)
            {
                if (m != this && CanBeHarmful(m, false))
                    potential.Add(m);
            }
            nearby.Free();

            int shots = Math.Min(4, potential.Count);
            for (int i = 0; i < shots; i++)
            {
                var target = potential[Utility.Random(potential.Count)];
                DoHarmful(target);

                // Visual bolt
                Effects.SendMovingParticles(
                    new Entity(Serial.Zero, this.Location, this.Map),
                    new Entity(Serial.Zero, target.Location, target.Map),
                    0x3818, 7, 0, false, false, UniqueHue, 0, 9502, 1, 0, EffectLayer.Waist, 0);

                Timer.DelayCall(TimeSpan.FromSeconds(0.2 * i), () =>
                {
                    if (target.Alive && CanBeHarmful(target, false))
                    {
                        int dmg = Utility.RandomMinMax(30, 45);
                        AOS.Damage(target, this, dmg, 0, 0, 0, 0, 100);
                        if (target is Mobile mob)
                            mob.SendMessage(0x22, "An ethereal bolt pierces you!");
                    }
                });
            }
        }

        // —— Ability 3: Spawn Planar Rift ——  
        private void SpawnPlanarRift()
        {
            this.Say("*Phase rupture!*");
            PlaySound(0x22F);

            // Create a random hazard tile under itself
            Type[] tiles = new Type[]
            {
                typeof(VortexTile),
                typeof(ManaDrainTile),
                typeof(FlamestrikeHazardTile),
                typeof(LightningStormTile)
            };

            Type choice = tiles[Utility.Random(tiles.Length)];
            var tile = Activator.CreateInstance(choice, true) as IEntity;
            if (tile is Item itemTile)
            {
                itemTile.Hue = UniqueHue;
                itemTile.MoveToWorld(this.Location, this.Map);
            }
        }

        // —— Death Explosion ——  
        public override void OnDeath(Container c)
        {
            if (Map != null)
            {
                this.Say("*Reality… fracturing…*");
                Effects.PlaySound(Location, Map, 0x211);
                Effects.SendLocationParticles(
                    EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration),
                    0x3709, 10, 60, UniqueHue, 0, 5052, 0);

                // Scatter quicksand and rifts
                for (int i = 0; i < 6; i++)
                {
                    int dx = Utility.RandomMinMax(-3, 3);
                    int dy = Utility.RandomMinMax(-3, 3);
                    var loc = new Point3D(X + dx, Y + dy, Z);
                    if (!Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                        loc.Z = Map.GetAverageZ(loc.X, loc.Y);

                    var qs = new QuicksandTile() { Hue = UniqueHue };
                    qs.MoveToWorld(loc, Map);
                }
            }

            base.OnDeath(c);
        }

        // —— Loot ——  
        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 3);
            AddLoot(LootPack.HighScrolls, Utility.RandomMinMax(2, 4));
            AddLoot(LootPack.Gems, Utility.RandomMinMax(10, 15));

            if (Utility.RandomDouble() < 0.03) // 3% unique artifact
                PackItem(new WatchersKasaOfTheEastWinds());

            if (Utility.RandomDouble() < 0.10)
                PackItem(new VoidEssence(Utility.RandomMinMax(1, 3)));
        }

        // —— Serialization ——  
        public Phasebeak(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            // Re‑init cooldowns on reload
            var now = DateTime.UtcNow;
            m_NextSonicScreech = now + TimeSpan.FromSeconds(Utility.RandomMinMax(10,14));
            m_NextPhaseVolley   = now + TimeSpan.FromSeconds(Utility.RandomMinMax(18,24));
            m_NextRiftSpawn     = now + TimeSpan.FromSeconds(Utility.RandomMinMax(20,28));
        }
    }
}
