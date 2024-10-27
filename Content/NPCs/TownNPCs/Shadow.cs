using SonicArmor.Content.EmoteBubbles;
using SonicArmor.Content.Items.Armor;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.Personalities;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace SonicArmor.Content.NPCs.TownNPCs
{
    [AutoloadHead]
    public class SHADOW : ModNPC
    {
        private static int ShimmerHeadIndex;
	    private static Profiles.StackedNPCProfile NPCProfile;

	    public override void Load() {
	    ShimmerHeadIndex = Mod.AddNPCHeadTexture(Type, Texture + "_Shimmer_Head");
	    }
        public override void SetStaticDefaults()
        {
        Main.npcFrameCount[NPC.type] = 25;
        NPCID.Sets.ExtraFramesCount[NPC.type] = 0;
        NPCID.Sets.AttackFrameCount[NPC.type] = 2;
        NPCID.Sets.DangerDetectRange[NPC.type] = 700;
        NPCID.Sets.AttackType[NPC.type] = 1;
        NPCID.Sets.AttackTime[NPC.type] = 50;
        NPCID.Sets.AttackAverageChance[NPC.type] = 15;
		NPCID.Sets.FaceEmote[Type] = ModContent.EmoteBubbleType<SonicEmote>();
        NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers() {
        Velocity = 1f,
        };
        NPC.Happiness
				.SetBiomeAffection<SnowBiome>(AffectionLevel.Like)
				.SetBiomeAffection<OceanBiome>(AffectionLevel.Hate)
				.SetBiomeAffection<ForestBiome>(AffectionLevel.Love)
                .SetNPCAffection(NPCID.BestiaryGirl, AffectionLevel.Like)
				.SetNPCAffection(NPCID.Mechanic, AffectionLevel.Like)
				.SetNPCAffection(NPCID.GoblinTinkerer, AffectionLevel.Like)
				.SetNPCAffection(NPCID.Merchant, AffectionLevel.Dislike)
				.SetNPCAffection(NPCID.Demolitionist, AffectionLevel.Hate);
        }
        public override void SetDefaults(){
        NPC.townNPC = true;
        NPC.friendly = true;
        NPC.width = 18;
        NPC.height = 44;
        NPC.aiStyle = NPCAIStyleID.Passive;
        NPC.damage = 25;
        NPC.defense = 25;
        NPC.lifeMax = 450;
        NPC.HitSound = SoundID.NPCHit1;
        NPC.DeathSound = SoundID.NPCDeath1;
        NPC.knockBackResist = 0.5f;
        AnimationType = NPCID.Guide;
        }
		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) {
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
				new FlavorTextBestiaryInfoElement("Shadow is a black anthropomorphic hedgehog with red stripes capable of running faster than the speed of sound. Created in the Space Colony ARK, Shadow was created to achieve immortality. His trademark power is Chaos Control, which allows him to warp spacetime with a Chaos Emerald."),
			});
			
			;
			NPCProfile = new Profiles.StackedNPCProfile(
				new Profiles.DefaultNPCProfile(Texture, NPCHeadLoader.GetHeadSlot(HeadTexture), Texture + "_Party"),
				new Profiles.DefaultNPCProfile(Texture + "_Shimmer", ShimmerHeadIndex, Texture + "_Shimmer_Party")
			);
		}
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) {
			if (NPCID.Sets.NPCBestiaryDrawOffset.TryGetValue(Type, out NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers)) {
				drawModifiers.Rotation += 0.001f;
				NPCID.Sets.NPCBestiaryDrawOffset.Remove(Type);
				NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
			}
			return true;
		}

        public override bool CanTownNPCSpawn(int money)
        {
            for (var i = 0; i < 255; i++)
            {
                Player player = Main.player[i];
                foreach (Item item in player.inventory)
                {
                    if (item.type == ItemID.SpectreBoots)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public override List<string> SetNPCNameList()
        {
            return new List<string>()
            {
                "Shadow"
            };
        }

        public override void SetChatButtons(ref string button, ref string button2)
        {
            button = "Shop";
            button2 = "Second Button";
        }

        public override void OnChatButtonClicked(bool firstButton, ref string shopName)
        {
            if (firstButton)
            {
                shopName = "Shop";
            }
        }
        public override void AddShops()
        {
            NPCShop shop = new(Type);
                .AddWithCustomValue(ModContent.ItemType<SonicBreastplate>(), Item.buyPrice(silver: 15))
                .AddWithCustomValue(ModContent.ItemType<SonicBreastplate>(), Item.buyPrice(silver: 15))
                .AddWithCustomValue(ModContent.ItemType<SonicLeggings>(), Item.buyPrice(silver: 15))
                .Register();
        }
        public override void HitEffect(NPC.HitInfo hit)
        {
            if (NPC.life <= 0)
            {
                if (Main.netMode != NetmodeID.Server)
                {
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Shadow").Type, 1f);
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Shadow2").Type, 1f);
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Shadow3").Type, 1f);
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Shadow4").Type, 1f);
                }
            }
        }

        public override string GetChat(){
if (NPC.homeless)
            return this.GetLocalizedValue("Chat.NoHome" + Main.rand.Next(1, 2 + 1));
				
                WeightedRandom<string> dialogue = new WeightedRandom<string>();

            dialogue.Add(this.GetLocalizedValue("Chat.Normal1"));
            dialogue.Add(this.GetLocalizedValue("Chat.Normal2"));
            dialogue.Add(this.GetLocalizedValue("Chat.Normal3"));
            return dialogue;
        }

        public override void TownNPCAttackStrength(ref int damage, ref float knockback)
        {
            damage = 50;
            knockback = 2f;
        }

        public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown)
        {
            cooldown = 5;
            randExtraCooldown = 10;
        }
        public override void TownNPCAttackProj(ref int projType, ref int attackDelay)
        {
            projType = ProjectileID.WoodenArrowFriendly;
            attackDelay = 1;
        }

        public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
        {
            multiplier = 6f;
        }
    }
}